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

namespace Connector.Sessions.v1.MetadataBlock;

public class MetadataBlockDataReader : TypedAsyncDataReaderBase<MetadataBlockDataObject>
{
    private readonly ILogger<MetadataBlockDataReader> _logger;
    private readonly ApiClient _apiClient;
    private int _currentPage = 0;

    public MetadataBlockDataReader(
        ILogger<MetadataBlockDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<MetadataBlockDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (true)
        {
            ApiResponse<PaginatedResponse<MetadataBlockDataObject>> response;
            
            try
            {
                response = await _apiClient.GetRecords<MetadataBlockDataObject>(
                    relativeUrl: "sessions/files",
                    page: _currentPage,
                    cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (HttpRequestException exception)
            {
                _logger.LogError(exception, "Exception while retrieving metadata blocks");
                throw;
            }

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve metadata blocks. API StatusCode: {response.StatusCode}");
            }

            if (response.Data?.Items == null || !response.Data.Items.Any())
            {
                yield break;
            }

            foreach (var block in response.Data.Items)
            {
                yield return block;
            }

            _currentPage++;
            if (_currentPage >= response.Data.TotalPages)
            {
                break;
            }
        }
    }
}