using Connector.Client;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.Notifications.v1.Subscriptions;

public class SubscriptionsDataReader : TypedAsyncDataReaderBase<SubscriptionsDataObject>
{
    private readonly ILogger<SubscriptionsDataReader> _logger;
    private readonly ApiClient _apiClient;

    public SubscriptionsDataReader(
        ILogger<SubscriptionsDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<SubscriptionsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ApiResponse<SubscriptionsDataObject> response;
        try
        {
            response = await _apiClient.GetSubscriptions(cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to retrieve subscriptions. API StatusCode: {StatusCode}", response.StatusCode);
                throw new Exception($"Failed to retrieve subscriptions. API StatusCode: {response.StatusCode}");
            }

            if (response.Data == null)
            {
                _logger.LogWarning("No subscriptions data returned from API");
                yield break;
            }
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while making a read request to data object 'SubscriptionsDataObject'");
            throw;
        }

        yield return response.Data;
    }
}