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

namespace Connector.Sessions.v1.FileToAWS;

public class FileToAWSDataReader : TypedAsyncDataReaderBase<FileToAWSDataObject>
{
    private readonly ILogger<FileToAWSDataReader> _logger;
    private readonly ApiClient _apiClient;
    private int _currentPage = 0;

    public FileToAWSDataReader(
        ILogger<FileToAWSDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<FileToAWSDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (true)
        {
            ApiResponse<PaginatedResponse<FileToAWSDataObject>> response;
            
            try
            {
                response = await _apiClient.GetRecords<FileToAWSDataObject>(
                    relativeUrl: "sessions/files/aws-uploads",
                    page: _currentPage,
                    cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (HttpRequestException exception)
            {
                _logger.LogError(exception, "Exception while retrieving AWS file upload data");
                throw;
            }

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve AWS file upload data. API StatusCode: {response.StatusCode}");
            }

            if (response.Data?.Items == null || !response.Data.Items.Any())
            {
                yield break;
            }

            foreach (var upload in response.Data.Items)
            {
                if (upload.Status != null)
                {
                    yield return upload;
                }
                else
                {
                    _logger.LogWarning("Skipping upload record with null status");
                }
            }

            _currentPage++;
            if (_currentPage >= response.Data.TotalPages)
            {
                break;
            }
        }
    }
}