namespace Connector.Sessions.v1.Snapshop.Create;

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
[Description("Creates a snapshot of a file in a Bluebeam Studio Session, combining PDF content with markup layer")]
public class CreateSnapshopAction : IStandardAction<CreateSnapshopActionInput, CreateSnapshopActionOutput>
{
    public CreateSnapshopActionInput ActionInput { get; set; } = new() { SessionId = string.Empty, FileId = 0 };
    public CreateSnapshopActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class CreateSnapshopActionInput
{
    [JsonPropertyName("sessionId")]
    [Description("The ID of the session containing the file")]
    [Required]
    public required string SessionId { get; init; }

    [JsonPropertyName("fileId")]
    [Description("The ID of the file to create a snapshot for")]
    [Required]
    public required int FileId { get; init; }
}

public class CreateSnapshopActionOutput
{
    [JsonPropertyName("status")]
    [Description("The status of the snapshot creation")]
    public string Status { get; init; } = "Requested";

    [JsonPropertyName("statusTime")]
    [Description("The timestamp when the status was last updated")]
    public string StatusTime { get; init; } = string.Empty;
}
