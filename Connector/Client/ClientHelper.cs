namespace Connector.Client
{
    using Connector.Connections;
    using ESR.Hosting.Client;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Text.Json;
    using Xchange.Connector.SDK.Client.AuthTypes;
    using Xchange.Connector.SDK.Client.AuthTypes.DelegatingHandlers;
    using Xchange.Connector.SDK.Client.ConnectivityApi.Models;

    public static class ClientHelper
    {
        public static class AuthTypeKeyEnums
        {
            public const string OAuth2CodeFlow = "oAuth2CodeFlow";
        }

        public static void ResolveServices(this IServiceCollection serviceCollection, ConnectionContainer activeConnection)
        {
            serviceCollection.AddTransient<RetryPolicyHandler>();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            switch (activeConnection.DefinitionKey)
            {
                case AuthTypeKeyEnums.OAuth2CodeFlow:
                    var configOAuth2CodeFlow = JsonSerializer.Deserialize<OAuth2CodeFlow>(activeConnection.Configuration, options);
                    serviceCollection.AddSingleton<OAuth2CodeFlowBase>(configOAuth2CodeFlow!);
                    serviceCollection.AddTransient<RetryPolicyHandler>();
                    serviceCollection.AddTransient<OAuth2CodeFlowHandler>();
                    serviceCollection.AddHttpClient<ApiClient, ApiClient>(client => new ApiClient(client, configOAuth2CodeFlow!.BaseUrl)).AddHttpMessageHandler<OAuth2CodeFlowHandler>().AddHttpMessageHandler<RetryPolicyHandler>();
                    break;
                default:
                    throw new Exception($"Unable to find services for definition key {activeConnection.DefinitionKey}");
            }
        }
    }
}