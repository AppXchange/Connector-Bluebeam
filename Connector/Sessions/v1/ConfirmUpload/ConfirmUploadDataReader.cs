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

namespace Connector.Sessions.v1.ConfirmUpload;

public class ConfirmUploadDataReader : TypedAsyncDataReaderBase<ConfirmUploadDataObject>
{
    private readonly ILogger<ConfirmUploadDataReader> _logger;
    private readonly ApiClient _apiClient;
    private int _currentPage = 0;

    public ConfirmUploadDataReader(
        ILogger<ConfirmUploadDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<ConfirmUploadDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (true)
        {
            ApiResponse<PaginatedResponse<ConfirmUploadDataObject>> response;
            
            try
            {
                response = await _apiClient.GetRecords<ConfirmUploadDataObject>(
                    relativeUrl: "sessions/files/confirmed",
                    page: _currentPage,
                    cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (HttpRequestException exception)
            {
                _logger.LogError(exception, "Exception while retrieving confirmed uploads");
                throw;
            }

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve confirmed uploads. API StatusCode: {response.StatusCode}");
            }

            if (response.Data?.Items == null || !response.Data.Items.Any())
            {
                yield break;
            }

            foreach (var confirmation in response.Data.Items)
            {
                yield return confirmation;
            }

            _currentPage++;
            if (_currentPage >= response.Data.TotalPages)
            {
                break;
            }
        }
    }
}