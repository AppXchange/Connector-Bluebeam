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

namespace Connector.Sessions.v1.Session;

public class SessionDataReader : TypedAsyncDataReaderBase<SessionDataObject>
{
    private readonly ILogger<SessionDataReader> _logger;
    private readonly ApiClient _apiClient;
    private int _currentPage = 0;

    public SessionDataReader(
        ILogger<SessionDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<SessionDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ApiResponse<PaginatedResponse<SessionDataObject>> response;
        
        try
        {
            response = await _apiClient.GetRecords<SessionDataObject>(
                relativeUrl: "sessions",
                page: _currentPage,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while retrieving sessions");
            throw;
        }

        if (!response.IsSuccessful)
        {
            throw new Exception($"Failed to retrieve sessions. API StatusCode: {response.StatusCode}");
        }

        if (response.Data?.Items == null || !response.Data.Items.Any())
        {
            yield break;
        }

        foreach (var session in response.Data.Items)
        {
            yield return session;
        }
    }
}