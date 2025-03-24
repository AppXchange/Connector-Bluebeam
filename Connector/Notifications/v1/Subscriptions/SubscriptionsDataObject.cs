namespace Connector.Notifications.v1.Subscriptions;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;
using System.Collections.Generic;

/// <summary>
/// Data object that will represent an object in the Xchange system. This will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[PrimaryKey("id", nameof(Id))]
//[AlternateKey("alt-key-id", nameof(CompanyId), nameof(EquipmentNumber))]
[Description("Represents a collection of Bluebeam Studio subscriptions")]
public class SubscriptionsDataObject
{
    [JsonPropertyName("id")]
    [Description("The unique identifier for the subscriptions collection")]
    [Required]
    public required string Id { get; init; }

    [JsonPropertyName("subscriptions")]
    [Description("List of subscription items")]
    public List<SubscriptionItem> Subscriptions { get; init; } = new();

    [JsonPropertyName("totalCount")]
    [Description("Total number of subscriptions")]
    public int TotalCount { get; init; }
}

public class SubscriptionItem
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
    public int SubscriptionId { get; init; }

    [JsonPropertyName("uri")]
    [Description("The callback URI")]
    public string Uri { get; init; } = string.Empty;
}