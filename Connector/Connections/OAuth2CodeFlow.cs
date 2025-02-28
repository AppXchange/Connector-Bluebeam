using System;
using Xchange.Connector.SDK.Client.AuthTypes;
using Xchange.Connector.SDK.Client.ConnectionDefinitions.Attributes;

namespace Connector.Connections;

[ConnectionDefinition(title: "OAuth2CodeFlow", description: "")]
public class OAuth2CodeFlow : OAuth2CodeFlowBase
{
    [ConnectionProperty(title: "Connection Environment", description: "", isRequired: true, isSensitive: false)]
    public ConnectionEnvironmentOAuth2CodeFlow ConnectionEnvironment { get; set; } = ConnectionEnvironmentOAuth2CodeFlow.Unknown;

    public string BaseUrl
    {
        get
        {
            switch (ConnectionEnvironment)
            {
                case ConnectionEnvironmentOAuth2CodeFlow.Production:
                    return "http://prod.example.com";
                case ConnectionEnvironmentOAuth2CodeFlow.Test:
                    return "http://test.example.com";
                default:
                    throw new Exception("No base url was set.");
            }
        }
    }
}

public enum ConnectionEnvironmentOAuth2CodeFlow
{
    Unknown = 0,
    Production = 1,
    Test = 2
}