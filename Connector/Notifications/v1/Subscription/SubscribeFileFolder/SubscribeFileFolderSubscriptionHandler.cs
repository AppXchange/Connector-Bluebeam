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

namespace Connector.Notifications.v1.Subscription.SubscribeFileFolder;

public class SubscribeFileFolderSubscriptionHandler : IActionHandler<SubscribeFileFolderSubscriptionAction>
{
    private readonly ILogger<SubscribeFileFolderSubscriptionHandler> _logger;
    private readonly ApiClient _apiClient;

    public SubscribeFileFolderSubscriptionHandler(
        ILogger<SubscribeFileFolderSubscriptionHandler> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<SubscribeFileFolderSubscriptionActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[] { new Xchange.Connector.SDK.Action.Error
                {
                    Source = new[] { "SubscribeFileFolderSubscriptionHandler" },
                    Text = "Invalid input: Failed to deserialize action input"
                }}
            });
        }

        try
        {
            var response = await _apiClient.SubscribeToFileFolder(input, cancellationToken);
            if (!response.IsSuccessful)
            {
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = response.StatusCode.ToString(),
                    Errors = new[] { new Xchange.Connector.SDK.Action.Error
                    {
                        Source = new[] { "SubscribeFileFolderSubscriptionHandler" },
                        Text = "Failed to subscribe to file/folder"
                    }}
                });
            }

            if (response.Data == null)
            {
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = "500",
                    Errors = new[] { new Xchange.Connector.SDK.Action.Error
                    {
                        Source = new[] { "SubscribeFileFolderSubscriptionHandler" },
                        Text = "No subscription data returned from API"
                    }}
                });
            }

            // Convert the action output to a data object for caching
            var subscriptionDataObject = new SubscriptionDataObject
            {
                Id = response.Data.Id,
                EndpointId = response.Data.EndpointId,
                Resource = response.Data.Resource,
                Source = response.Data.Source,
                Status = response.Data.Status,
                SubscriptionId = response.Data.SubscriptionId,
                Uri = response.Data.Uri
            };

            // Build sync operations to update the cache
            var operations = new List<SyncOperation>();
            var keyResolver = new DefaultDataObjectKey();
            var key = keyResolver.BuildKeyResolver()(subscriptionDataObject);
            operations.Add(SyncOperation.CreateSyncOperation(UpdateOperation.Upsert.ToString(), key.UrlPart, key.PropertyNames, subscriptionDataObject));

            var resultList = new List<CacheSyncCollection>
            {
                new() { DataObjectType = typeof(SubscriptionDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(response.Data, resultList);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Failed to subscribe to file/folder");
            
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = exception.StatusCode?.ToString() ?? "500",
                Errors = new[] { new Xchange.Connector.SDK.Action.Error
                {
                    Source = new[] { "SubscribeFileFolderSubscriptionHandler" },
                    Text = exception.Message
                }}
            });
        }
    }
}
