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

namespace Connector.Sessions.v1.Download;

public class DownloadDataReader : TypedAsyncDataReaderBase<DownloadDataObject>
{
    private readonly ILogger<DownloadDataReader> _logger;
    private readonly ApiClient _apiClient;
    private int _currentPage = 0;

    public DownloadDataReader(
        ILogger<DownloadDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<DownloadDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (true)
        {
            ApiResponse<PaginatedResponse<DownloadDataObject>> response;
            
            try
            {
                response = await _apiClient.GetRecords<DownloadDataObject>(
                    relativeUrl: "sessions/downloads",
                    page: _currentPage,
                    cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (HttpRequestException exception)
            {
                _logger.LogError(exception, "Exception while retrieving snapshot download status");
                throw;
            }

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve snapshot download status. API StatusCode: {response.StatusCode}");
            }

            if (response.Data?.Items == null || !response.Data.Items.Any())
            {
                yield break;
            }

            foreach (var download in response.Data.Items)
            {
                yield return download;
            }

            _currentPage++;
            if (_currentPage >= response.Data.TotalPages)
            {
                break;
            }
        }
    }
}