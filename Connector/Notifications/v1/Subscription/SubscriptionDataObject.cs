namespace Connector.Notifications.v1.Subscription;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object that will represent an object in the Xchange system. This will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[PrimaryKey("subscriptionId", nameof(SubscriptionId))]
[Description("Represents a Bluebeam Studio subscription")]
public class SubscriptionDataObject
{
    [JsonPropertyName("id")]
    [Description("The unique identifier for the subscription item")]
    public required string Id { get; init; }

    [JsonPropertyName("endpointId")]
    [Description("The endpoint identifier")]
    public int EndpointId { get; init; }

    [JsonPropertyName("resource")]
    [Description("The resource path")]
    public string Resource { get; init; } = string.Empty;

    [JsonPropertyName("source")]
    [Description("The source of the subscription")]
    public string Source { get; init; } = string.Empty;

    [JsonPropertyName("status")]
    [Description("The status of the subscription")]
    public string Status { get; init; } = string.Empty;

    [JsonPropertyName("subscriptionId")]
    [Description("The subscription identifier")]
    [Required]
    public int SubscriptionId { get; init; }

    [JsonPropertyName("uri")]
    [Description("The callback URI")]
    public string Uri { get; init; } = string.Empty;
}