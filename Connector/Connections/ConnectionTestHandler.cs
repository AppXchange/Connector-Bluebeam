using Connector.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Xchange.Connector.SDK.Client.Testing;

namespace Connector.Connections
{
    public class ConnectionTestHandler : IConnectionTestHandler
    {
        private readonly ILogger<IConnectionTestHandler> _logger;
        private readonly ApiClient _apiClient;

        public ConnectionTestHandler(ILogger<IConnectionTestHandler> logger, ApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        public async Task<TestConnectionResult> TestConnection()
        {
            // Make a call to your API/system to obtain the connection test result.

            var response = await _apiClient.TestConnection();

            // Depending on the response, make your own specific messages.

            if (response == null)
            {
                return new TestConnectionResult()
                {
                    Success = false,
                    Message = "Failed to get response from server",
                    StatusCode = 500
                };
            }

            if (response.IsSuccessful)
            {
                return new TestConnectionResult()
                {
                    Success = true,
                    Message = "Successful test.",
                    StatusCode = response.StatusCode
                };
            }

            switch (response.StatusCode)
            {
                case 403:
                    return new TestConnectionResult()
                    {
                        Success = false,
                        Message = "Invalid Credentials: Forbidden.",
                        StatusCode = response.StatusCode
                    };
                case 401:
                    return new TestConnectionResult()
                    {
                        Success = false,
                        Message = "Invalid Credentials: Unauthorized",
                        StatusCode = response.StatusCode
                    };
                default:
                    return new TestConnectionResult()
                    {
                        Success = false,
                        Message = "Unknown Issue.",
                        StatusCode = response.StatusCode
                    };
            }
        }
    }
}
