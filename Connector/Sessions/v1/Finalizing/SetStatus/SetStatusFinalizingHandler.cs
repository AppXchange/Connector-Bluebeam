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

namespace Connector.Sessions.v1.Finalizing.SetStatus;

public class SetStatusFinalizingHandler : IActionHandler<SetStatusFinalizingAction>
{
    private readonly ILogger<SetStatusFinalizingHandler> _logger;
    private readonly ApiClient _apiClient;

    public SetStatusFinalizingHandler(
        ILogger<SetStatusFinalizingHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<SetStatusFinalizingActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[] { new Xchange.Connector.SDK.Action.Error
                {
                    Source = new[] { "SetStatusFinalizingHandler" },
                    Text = "Invalid input: Failed to deserialize action input"
                }}
            });
        }

        try
        {
            var response = await _apiClient.SetStatusFinalizing(input.SessionId, cancellationToken);

            if (!response.IsSuccessful)
            {
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = response.StatusCode.ToString(),
                    Errors = new[]
                    {
                        new Xchange.Connector.SDK.Action.Error
                        {
                            Source = new[] { "SetStatusFinalizingHandler" },
                            Text = "Failed to set session status to Finalizing"
                        }
                    }
                });
            }

            var output = new SetStatusFinalizingActionOutput();
            var finalizingData = new FinalizingDataObject
            {
                SessionId = input.SessionId,
                Status = "Finalizing",
                FinalizingTime = System.DateTime.UtcNow
            };

            var operations = new List<SyncOperation>();
            var keyResolver = new DefaultDataObjectKey();
            var key = keyResolver.BuildKeyResolver()(finalizingData);
            operations.Add(SyncOperation.CreateSyncOperation(UpdateOperation.Upsert.ToString(), key.UrlPart, key.PropertyNames, finalizingData));

            var resultList = new List<CacheSyncCollection>
            {
                new() { DataObjectType = typeof(FinalizingDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(output, resultList);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Failed to set session status to Finalizing");
            
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = exception.StatusCode?.ToString() ?? "500",
                Errors = new[]
                {
                    new Xchange.Connector.SDK.Action.Error
                    {
                        Source = new[] { "SetStatusFinalizingHandler" },
                        Text = exception.Message
                    }
                }
            });
        }
    }
}
