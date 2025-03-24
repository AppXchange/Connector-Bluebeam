using System;
using Xchange.Connector.SDK.Client.AuthTypes;
using Xchange.Connector.SDK.Client.ConnectionDefinitions.Attributes;

namespace Connector.Connections;

[ConnectionDefinition(title: "Bluebeam OAuth2", description: "OAuth2 Code Flow authentication for Bluebeam Studio API")]
public class OAuth2CodeFlow : OAuth2CodeFlowBase
{
    [ConnectionProperty(title: "Connection Environment", description: "Select the Bluebeam environment", isRequired: true, isSensitive: false)]
    public ConnectionEnvironmentOAuth2CodeFlow ConnectionEnvironment { get; set; } = ConnectionEnvironmentOAuth2CodeFlow.Unknown;

    public OAuth2CodeFlow()
    {
        // Set the OAuth2 endpoints for Bluebeam
        AuthorizationUrl = "https://authserver.bluebeam.com/auth/oauth/authorize";
        TokenUrl = "https://authserver.bluebeam.com/auth/token";
        RefreshUrl = "https://authserver.bluebeam.com/auth/token";
        
        // Set client authentication to use credentials in body as per Bluebeam's requirements
        ClientAuthentication = ClientAuthentication.ClientCredentialsInBody;
        
        // Set default scope
        Scope = "full_user jobs";
    }

    public string BaseUrl
    {
        get
        {
            switch (ConnectionEnvironment)
            {
                case ConnectionEnvironmentOAuth2CodeFlow.Production:
                    return "https://studioapi.bluebeam.com/publicapi/v1/";
                case ConnectionEnvironmentOAuth2CodeFlow.Test:
                    return "https://studioapi.bluebeam.com/publicapi/v1/";
                default:
                    throw new Exception("No base url was set.");
            }
        }
    }

    // Optional: Add any additional Bluebeam-specific properties if needed
}

public enum ConnectionEnvironmentOAuth2CodeFlow
{
    Unknown = 0,
    Production = 1,
    Test = 2
}