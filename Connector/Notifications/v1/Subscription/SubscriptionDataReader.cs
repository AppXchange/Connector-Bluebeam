using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Extensions.Logging;
using Xchange.Connector.SDK.CacheWriter;
using Connector.Client;
using ESR.Hosting.CacheWriter;

namespace Connector.Notifications.v1.Subscription;

public class SubscriptionDataReader : TypedAsyncDataReaderBase<SubscriptionDataObject>
{
    private readonly ILogger<SubscriptionDataReader> _logger;
    private readonly ApiClient _apiClient;

    public SubscriptionDataReader(ILogger<SubscriptionDataReader> logger, ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<SubscriptionDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        SubscriptionDataObject? data = null;
        try
        {
            // Get the subscription ID from the URL parameters
            if (dataObjectRunArguments == null || !int.TryParse(dataObjectRunArguments.ToString(), out var subscriptionId))
            {
                _logger.LogError("Invalid or missing subscription ID in URL parameters");
                yield break;
            }

            var response = await _apiClient.GetSubscription(subscriptionId, cancellationToken);
            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to get subscription data. Status code: {StatusCode}", response.StatusCode);
                yield break;
            }

            if (response.Data == null)
            {
                _logger.LogWarning("No subscription data returned from API");
                yield break;
            }

            data = response.Data;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while retrieving subscription data");
            throw;
        }

        if (data != null)
        {
            yield return data;
        }
    }
}