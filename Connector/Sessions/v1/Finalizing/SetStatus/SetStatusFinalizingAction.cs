namespace Connector.Sessions.v1.Finalizing.SetStatus;

using Json.Schema.Generation;
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
[Description("Sets a Bluebeam Studio Session's status to Finalizing")]
public class SetStatusFinalizingAction : IStandardAction<SetStatusFinalizingActionInput, SetStatusFinalizingActionOutput>
{
    public SetStatusFinalizingActionInput ActionInput { get; set; } = new() { SessionId = string.Empty };
    public SetStatusFinalizingActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class SetStatusFinalizingActionInput
{
    [JsonPropertyName("sessionId")]
    [Description("The ID of the session to set to Finalizing status")]
    [Required]
    public required string SessionId { get; init; }
}

public class SetStatusFinalizingActionOutput
{
    [JsonPropertyName("status")]
    [Description("The new status of the session")]
    public string Status { get; init; } = "Finalizing";
}
