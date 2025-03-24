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

namespace Connector.Sessions.v1.Snapshop.Create;

public class CreateSnapshopHandler : IActionHandler<CreateSnapshopAction>
{
    private readonly ILogger<CreateSnapshopHandler> _logger;
    private readonly ApiClient _apiClient;

    public CreateSnapshopHandler(
        ILogger<CreateSnapshopHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<CreateSnapshopActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[] { new Xchange.Connector.SDK.Action.Error
                {
                    Source = new[] { "CreateSnapshopHandler" },
                    Text = "Invalid input: Failed to deserialize action input"
                }}
            });
        }

        try
        {
            var response = await _apiClient.CreateSnapshot(input.SessionId, input.FileId, cancellationToken);

            if (!response.IsSuccessful)
            {
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = response.StatusCode.ToString(),
                    Errors = new[]
                    {
                        new Xchange.Connector.SDK.Action.Error
                        {
                            Source = new[] { "CreateSnapshopHandler" },
                            Text = "Failed to create snapshot"
                        }
                    }
                });
            }

            var output = new CreateSnapshopActionOutput
            {
                Status = "Requested",
                StatusTime = System.DateTime.UtcNow.ToString("O")
            };

            var snapshopData = new SnapshopDataObject
            {
                SessionId = input.SessionId,
                FileId = input.FileId,
                Status = output.Status,
                StatusTime = System.DateTime.Parse(output.StatusTime)
            };

            var operations = new List<SyncOperation>();
            var keyResolver = new DefaultDataObjectKey();
            var key = keyResolver.BuildKeyResolver()(snapshopData);
            operations.Add(SyncOperation.CreateSyncOperation(UpdateOperation.Upsert.ToString(), key.UrlPart, key.PropertyNames, snapshopData));

            var resultList = new List<CacheSyncCollection>
            {
                new() { DataObjectType = typeof(SnapshopDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(output, resultList);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Failed to create snapshot");
            
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = exception.StatusCode?.ToString() ?? "500",
                Errors = new[]
                {
                    new Xchange.Connector.SDK.Action.Error
                    {
                        Source = new[] { "CreateSnapshopHandler" },
                        Text = exception.Message
                    }
                }
            });
        }
    }
}
