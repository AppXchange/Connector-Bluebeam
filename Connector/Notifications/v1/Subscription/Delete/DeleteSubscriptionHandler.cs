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

namespace Connector.Notifications.v1.Subscription.Delete;

public class DeleteSubscriptionHandler : IActionHandler<DeleteSubscriptionAction>
{
    private readonly ILogger<DeleteSubscriptionHandler> _logger;
    private readonly ApiClient _apiClient;

    public DeleteSubscriptionHandler(
        ILogger<DeleteSubscriptionHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<DeleteSubscriptionActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[] { new Xchange.Connector.SDK.Action.Error
                {
                    Source = new[] { "DeleteSubscriptionHandler" },
                    Text = "Invalid input: Failed to deserialize action input"
                }}
            });
        }

        try
        {
            var response = await _apiClient.DeleteSubscription(input.SubscriptionId, cancellationToken);
            if (!response.IsSuccessful)
            {
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = response.StatusCode.ToString(),
                    Errors = new[] { new Xchange.Connector.SDK.Action.Error
                    {
                        Source = new[] { "DeleteSubscriptionHandler" },
                        Text = "Failed to delete subscription"
                    }}
                });
            }

            var output = new DeleteSubscriptionActionOutput { Success = true };

            // Build sync operations to update the cache
            var operations = new List<SyncOperation>();
            var keyResolver = new DefaultDataObjectKey();
            var key = keyResolver.BuildKeyResolver()(new SubscriptionDataObject { Id = input.SubscriptionId.ToString(), SubscriptionId = input.SubscriptionId });
            operations.Add(SyncOperation.CreateSyncOperation(UpdateOperation.Delete.ToString(), key.UrlPart, key.PropertyNames, null));

            var resultList = new List<CacheSyncCollection>
            {
                new() { DataObjectType = typeof(SubscriptionDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(output, resultList);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Failed to delete subscription");
            
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = exception.StatusCode?.ToString() ?? "500",
                Errors = new[] { new Xchange.Connector.SDK.Action.Error
                {
                    Source = new[] { "DeleteSubscriptionHandler" },
                    Text = exception.Message
                }}
            });
        }
    }
}
