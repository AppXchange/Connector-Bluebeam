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

namespace Connector.Sessions.v1.Finalizing;

public class FinalizingDataReader : TypedAsyncDataReaderBase<FinalizingDataObject>
{
    private readonly ILogger<FinalizingDataReader> _logger;
    private readonly ApiClient _apiClient;
    private int _currentPage = 0;

    public FinalizingDataReader(
        ILogger<FinalizingDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<FinalizingDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (true)
        {
            ApiResponse<PaginatedResponse<FinalizingDataObject>> response;
            
            try
            {
                response = await _apiClient.GetRecords<FinalizingDataObject>(
                    relativeUrl: "sessions/finalizing",
                    page: _currentPage,
                    cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (HttpRequestException exception)
            {
                _logger.LogError(exception, "Exception while retrieving finalizing sessions");
                throw;
            }

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve finalizing sessions. API StatusCode: {response.StatusCode}");
            }

            if (response.Data?.Items == null || !response.Data.Items.Any())
            {
                yield break;
            }

            foreach (var session in response.Data.Items)
            {
                yield return session;
            }

            _currentPage++;
            if (_currentPage >= response.Data.TotalPages)
            {
                break;
            }
        }
    }
}