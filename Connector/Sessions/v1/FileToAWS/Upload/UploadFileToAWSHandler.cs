using Connector.Client;
using ESR.Hosting.Action;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xchange.Connector.SDK.Action;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.Sessions.v1.FileToAWS.Upload;

public class UploadFileToAWSHandler : IActionHandler<UploadFileToAWSAction>
{
    private readonly ILogger<UploadFileToAWSHandler> _logger;
    private readonly HttpClient _httpClient;

    public UploadFileToAWSHandler(
        ILogger<UploadFileToAWSHandler> logger,
        HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<UploadFileToAWSActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[] { new Xchange.Connector.SDK.Action.Error
                {
                    Source = new[] { "UploadFileToAWSHandler" },
                    Text = "Invalid input: Failed to deserialize action input"
                }}
            });
        }

        try
        {
            // Download file from source URL
            var fileResponse = await _httpClient.GetAsync(input.FileUrl, cancellationToken);
            if (!fileResponse.IsSuccessStatusCode)
            {
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = fileResponse.StatusCode.ToString(),
                    Errors = new[] { new Xchange.Connector.SDK.Action.Error
                    {
                        Source = new[] { "UploadFileToAWSHandler" },
                        Text = $"Failed to download file from source URL: {fileResponse.StatusCode}"
                    }}
                });
            }

            // Upload file to AWS
            var fileContent = await fileResponse.Content.ReadAsStreamAsync(cancellationToken);
            var uploadRequest = new HttpRequestMessage(HttpMethod.Put, input.UploadUrl)
            {
                Content = new StreamContent(fileContent)
            };
            uploadRequest.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            uploadRequest.Headers.Add("x-amz-server-side-encryption", "AES256");

            var uploadResponse = await _httpClient.SendAsync(uploadRequest, cancellationToken);
            if (!uploadResponse.IsSuccessStatusCode)
            {
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = uploadResponse.StatusCode.ToString(),
                    Errors = new[] { new Xchange.Connector.SDK.Action.Error
                    {
                        Source = new[] { "UploadFileToAWSHandler" },
                        Text = $"Failed to upload file to AWS: {uploadResponse.StatusCode}"
                    }}
                });
            }

            var output = new UploadFileToAWSActionOutput
            {
                Success = true
            };

            return ActionHandlerOutcome.Successful(output);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Error during file upload to AWS");
            
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = exception.StatusCode?.ToString() ?? "500",
                Errors = new[] { new Xchange.Connector.SDK.Action.Error
                {
                    Source = new[] { "UploadFileToAWSHandler" },
                    Text = exception.Message
                }}
            });
        }
    }
}
