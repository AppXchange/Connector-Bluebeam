namespace Connector.Notifications.v1.Subscription.SubscribeStudioProject;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Action object that will represent an action in the Xchange system. This will contain an input object type,
/// an output object type, and a Action failure type (this will default to <see cref="StandardActionFailure"/>
/// but that can be overridden with your own preferred type). These objects will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[Description("Subscribe to notifications for a Bluebeam Studio Project")]
public class SubscribeStudioProjectSubscriptionAction : IStandardAction<SubscribeStudioProjectSubscriptionActionInput, SubscribeStudioProjectSubscriptionActionOutput>
{
    public SubscribeStudioProjectSubscriptionActionInput ActionInput { get; set; } = new() { ProjectId = string.Empty, Uri = string.Empty };
    public SubscribeStudioProjectSubscriptionActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class SubscribeStudioProjectSubscriptionActionInput
{
    [JsonPropertyName("parentResourceType")]
    [Description("The parent object type for the subscription (Projects)")]
    [Required]
    public string ParentResourceType { get; init; } = "Projects";

    [JsonPropertyName("projectId")]
    [Description("9-digit ID for the Studio Project")]
    [Required]
    public required string ProjectId { get; init; }

    [JsonPropertyName("uri")]
    [Description("The callback URI where notifications will be sent")]
    [Required]
    public required string Uri { get; init; }
}

public class SubscribeStudioProjectSubscriptionActionOutput
{
    [JsonPropertyName("$id")]
    [Description("The unique identifier for the response")]
    public string Id { get; init; } = string.Empty;

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

    [JsonPropertyName("key")]
    [Description("The subscription key")]
    public string Key { get; init; } = string.Empty;
}
