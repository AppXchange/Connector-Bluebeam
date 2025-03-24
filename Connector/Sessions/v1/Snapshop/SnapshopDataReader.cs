using Connector.Client;
using System;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;
using System.Net.Http;

namespace Connector.Sessions.v1.Snapshop;

public class SnapshopDataReader : TypedAsyncDataReaderBase<SnapshopDataObject>
{
    private readonly ILogger<SnapshopDataReader> _logger;
    private readonly ApiClient _apiClient;
    private int _currentPage = 0;

    public SnapshopDataReader(
        ILogger<SnapshopDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<SnapshopDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (true)
        {
            ApiResponse<PaginatedResponse<SnapshopDataObject>> response;
            
            try
            {
                response = await _apiClient.GetRecords<SnapshopDataObject>(
                    relativeUrl: "sessions/snapshots",
                    page: _currentPage,
                    cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (HttpRequestException exception)
            {
                _logger.LogError(exception, "Exception while retrieving snapshots");
                throw;
            }

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve snapshots. API StatusCode: {response.StatusCode}");
            }

            if (response.Data?.Items == null || !response.Data.Items.Any())
            {
                yield break;
            }

            foreach (var snapshot in response.Data.Items)
            {
                yield return snapshot;
            }

            _currentPage++;
            if (_currentPage >= response.Data.TotalPages)
            {
                break;
            }
        }
    }
}