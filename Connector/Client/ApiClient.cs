using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Connector.Sessions.v1.Session.Initialize;
using Connector.Sessions.v1.MetadataBlock.Create;
using Connector.Sessions.v1.User.Invite;
using Connector.Sessions.v1.User.Add;
using Connector.Sessions.v1.Finalizing.SetStatus;
using Connector.Sessions.v1.Snapshop.Create;
using Connector.Sessions.v1.Download;
using Connector.Sessions.v1.FileToAWS.Upload;
using Connector.Sessions.v1.Session.Delete;
using Connector.Notifications.v1.Subscriptions;
using Connector.Notifications.v1.Subscription;
using Connector.Notifications.v1.Subscription.SubscribeStudioProject;
using Connector.Notifications.v1.Subscription.SubscribeFileFolder;
using Connector.Notifications.v1.Subscription.SubscribeStudioSession;
using Connector.Notifications.v1.Subscription.SubscribeDocument;
using Connector.Notifications.v1.ComprehensiveSubscription;
using Connector.Notifications.v1.ComprehensiveSubscription.Create;

namespace Connector.Client;

/// <summary>
/// A client for interfacing with the API via the HTTP protocol.
/// </summary>
public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient (HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new System.Uri(baseUrl);
    }

    // Example of a paginated response.
    public async Task<ApiResponse<PaginatedResponse<T>>> GetRecords<T>(string relativeUrl, int page, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{relativeUrl}?page={page}", cancellationToken: cancellationToken).ConfigureAwait(false);
        return new ApiResponse<PaginatedResponse<T>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<PaginatedResponse<T>>(cancellationToken: cancellationToken) : default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse> GetNoContent(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync("no-content", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse> TestConnection(CancellationToken cancellationToken = default)
    {
        // The purpose of this method is to validate that successful and authorized requests can be made to the API.
        // In this example, we are using the GET "oauth/me" endpoint.
        // Choose any endpoint that you consider suitable for testing the connection with the API.

        var response = await _httpClient
            .GetAsync($"oauth/me", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
        };
    }

    public async Task<ApiResponse<InitializeSessionActionOutput>> InitializeSession(
        InitializeSessionActionInput input,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "sessions",
            input,
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse<InitializeSessionActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<InitializeSessionActionOutput>(cancellationToken: cancellationToken) 
                : default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse<CreateMetadataBlockActionOutput>> CreateMetadataBlock(
        string sessionId,
        CreateMetadataBlockActionInput input,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"sessions/{sessionId}/files",
            new
            {
                input.Name,
                input.Source,
                input.Size,
                input.CRC
            },
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse<CreateMetadataBlockActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<CreateMetadataBlockActionOutput>(cancellationToken: cancellationToken) 
                : default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse> ConfirmUpload(
        string sessionId,
        int fileId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsync(
            $"sessions/{sessionId}/files/{fileId}/confirm-upload",
            null,
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse> InviteUser(
        InviteUserActionInput input,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"sessions/{input.SessionId}/invite",
            new
            {
                input.Email,
                input.Message
            },
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse> AddUser(
        AddUserActionInput input,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"sessions/{input.SessionId}/users",
            new
            {
                input.Email,
                input.SendEmail,
                input.Message
            },
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse> SetStatusFinalizing(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"sessions/{sessionId}",
            new { Status = "Finalizing" },
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse> CreateSnapshot(
        string sessionId,
        int fileId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsync(
            $"sessions/{sessionId}/files/{fileId}/snapshot",
            null,
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse<DownloadDataObject>> GetSnapshotStatus(
        string sessionId,
        int fileId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            $"sessions/{sessionId}/files/{fileId}/snapshot",
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse<DownloadDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<DownloadDataObject>(cancellationToken: cancellationToken) 
                : default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse<UploadFileToAWSActionOutput>> UploadFileToAWS(
        string sessionId,
        int fileId,
        UploadFileToAWSActionInput input,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"sessions/{sessionId}/files/{fileId}/upload",
            new
            {
                input.UploadUrl,
                input.FileUrl
            },
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse<UploadFileToAWSActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<UploadFileToAWSActionOutput>(cancellationToken: cancellationToken) 
                : default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse> DeleteSession(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync(
            $"sessions/{sessionId}",
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse<SubscriptionsDataObject>> GetSubscriptions(
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            "notifications/subscriptions",
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse<SubscriptionsDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<SubscriptionsDataObject>(cancellationToken: cancellationToken) 
                : default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse<SubscriptionDataObject>> GetSubscription(
        int subscriptionId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            $"notifications/subscriptions/{subscriptionId}",
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse<SubscriptionDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<SubscriptionDataObject>(cancellationToken: cancellationToken) 
                : default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse<SubscribeStudioProjectSubscriptionActionOutput>> SubscribeToStudioProject(
        SubscribeStudioProjectSubscriptionActionInput input,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "notifications/subscriptions",
            new
            {
                ParentResourceType = input.ParentResourceType,
                ProjectId = input.ProjectId,
                Uri = input.Uri
            },
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse<SubscribeStudioProjectSubscriptionActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<SubscribeStudioProjectSubscriptionActionOutput>(cancellationToken: cancellationToken) 
                : default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse<SubscribeFileFolderSubscriptionActionOutput>> SubscribeToFileFolder(
        SubscribeFileFolderSubscriptionActionInput input,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "notifications/subscriptions",
            new
            {
                ParentResourceType = input.ParentResourceType,
                ProjectId = input.ProjectId,
                ItemType = input.ItemType,
                ProjectItemId = input.ProjectItemId,
                Uri = input.Uri
            },
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse<SubscribeFileFolderSubscriptionActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<SubscribeFileFolderSubscriptionActionOutput>(cancellationToken: cancellationToken) 
                : default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse<SubscribeStudioSessionSubscriptionActionOutput>> SubscribeToStudioSession(
        SubscribeStudioSessionSubscriptionActionInput input,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "notifications/subscriptions",
            new
            {
                ParentResourceType = input.ParentResourceType,
                SessionId = input.SessionId,
                Uri = input.Uri
            },
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse<SubscribeStudioSessionSubscriptionActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<SubscribeStudioSessionSubscriptionActionOutput>(cancellationToken: cancellationToken) 
                : default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse<SubscribeDocumentSubscriptionActionOutput>> SubscribeToDocument(
        SubscribeDocumentSubscriptionActionInput input,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "notifications/subscriptions",
            new
            {
                ParentResourceType = input.ParentResourceType,
                SessionId = input.SessionId,
                SessionDocumentId = input.SessionDocumentId,
                Uri = input.Uri
            },
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse<SubscribeDocumentSubscriptionActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<SubscribeDocumentSubscriptionActionOutput>(cancellationToken: cancellationToken) 
                : default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse> DeleteSubscription(
        int subscriptionId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync(
            $"notifications/subscriptions/{subscriptionId}",
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse<CreateComprehensiveSubscriptionActionOutput>> CreateComprehensiveSubscription(
        CreateComprehensiveSubscriptionActionInput input,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "notifications/subscriptions/comprehensive",
            new
            {
                ParentResourceType = input.ParentResourceType,
                ProjectId = input.ProjectId,
                Uri = input.Uri
            },
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse<CreateComprehensiveSubscriptionActionOutput>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<CreateComprehensiveSubscriptionActionOutput>(cancellationToken: cancellationToken) 
                : default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    public async Task<ApiResponse<ComprehensiveSubscriptionDataObject>> GetComprehensiveSubscription(
        int subscriptionId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            $"notifications/subscriptions/comprehensive/{subscriptionId}",
            cancellationToken).ConfigureAwait(false);

        return new ApiResponse<ComprehensiveSubscriptionDataObject>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode 
                ? await response.Content.ReadFromJsonAsync<ComprehensiveSubscriptionDataObject>(cancellationToken: cancellationToken) 
                : default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }
}