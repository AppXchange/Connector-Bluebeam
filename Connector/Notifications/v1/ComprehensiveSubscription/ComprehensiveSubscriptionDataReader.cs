using Connector.Client;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.Notifications.v1.ComprehensiveSubscription;

public class ComprehensiveSubscriptionDataReader : TypedAsyncDataReaderBase<ComprehensiveSubscriptionDataObject>
{
    private readonly ILogger<ComprehensiveSubscriptionDataReader> _logger;
    private readonly ApiClient _apiClient;

    public ComprehensiveSubscriptionDataReader(
        ILogger<ComprehensiveSubscriptionDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<ComprehensiveSubscriptionDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ComprehensiveSubscriptionDataObject? data = null;
        try
        {
            // Get the subscription ID from the URL parameters
            if (dataObjectRunArguments == null || !int.TryParse(dataObjectRunArguments.ToString(), out var subscriptionId))
            {
                _logger.LogError("Invalid or missing subscription ID in URL parameters");
                yield break;
            }

            var response = await _apiClient.GetComprehensiveSubscription(subscriptionId, cancellationToken);
            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to get comprehensive subscription data. Status code: {StatusCode}", response.StatusCode);
                yield break;
            }

            if (response.Data == null)
            {
                _logger.LogWarning("No comprehensive subscription data returned from API");
                yield break;
            }

            data = response.Data;
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while retrieving comprehensive subscription data");
            throw;
        }

        if (data != null)
        {
            yield return data;
        }
    }
}