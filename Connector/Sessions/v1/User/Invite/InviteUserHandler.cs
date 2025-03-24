using Connector.Client;
using ESR.Hosting.Action;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xchange.Connector.SDK.Action;
using Xchange.Connector.SDK.CacheWriter;
using Xchange.Connector.SDK.Client.AppNetwork;

namespace Connector.Sessions.v1.User.Invite;

public class InviteUserHandler : IActionHandler<InviteUserAction>
{
    private readonly ILogger<InviteUserHandler> _logger;
    private readonly ApiClient _apiClient;

    public InviteUserHandler(
        ILogger<InviteUserHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<InviteUserActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[] { new Xchange.Connector.SDK.Action.Error
                {
                    Source = new[] { "InviteUserHandler" },
                    Text = "Invalid input: Failed to deserialize action input"
                }}
            });
        }

        try
        {
            var response = await _apiClient.InviteUser(input, cancellationToken);

            if (!response.IsSuccessful)
            {
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = response.StatusCode.ToString(),
                    Errors = new[]
                    {
                        new Xchange.Connector.SDK.Action.Error
                        {
                            Source = new[] { "InviteUserHandler" },
                            Text = "Failed to invite user"
                        }
                    }
                });
            }

            var output = new InviteUserActionOutput();
            var userData = new UserDataObject
            {
                Email = input.Email,
                SessionId = input.SessionId,
                Message = input.Message,
                InvitationStatus = "Invited",
                InvitationTime = System.DateTime.UtcNow
            };

            var operations = new List<SyncOperation>();
            var keyResolver = new DefaultDataObjectKey();
            var key = keyResolver.BuildKeyResolver()(userData);
            operations.Add(SyncOperation.CreateSyncOperation(UpdateOperation.Upsert.ToString(), key.UrlPart, key.PropertyNames, userData));

            var resultList = new List<CacheSyncCollection>
            {
                new() { DataObjectType = typeof(UserDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(output, resultList);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Failed to invite user");
            
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = exception.StatusCode?.ToString() ?? "500",
                Errors = new[]
                {
                    new Xchange.Connector.SDK.Action.Error
                    {
                        Source = new[] { "InviteUserHandler" },
                        Text = exception.Message
                    }
                }
            });
        }
    }
}
