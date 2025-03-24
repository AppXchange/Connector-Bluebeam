namespace Connector.Sessions.v1.User.Invite;

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
[Description("Invites a user to join a Bluebeam Studio Session")]
public class InviteUserAction : IStandardAction<InviteUserActionInput, InviteUserActionOutput>
{
    public InviteUserActionInput ActionInput { get; set; } = new() { Email = string.Empty, SessionId = string.Empty };
    public InviteUserActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class InviteUserActionInput
{
    [JsonPropertyName("sessionId")]
    [Description("The ID of the session to invite the user to")]
    [Required]
    public required string SessionId { get; init; }

    [JsonPropertyName("email")]
    [Description("Email address to send invitation to")]
    [Required]
    public required string Email { get; init; }

    [JsonPropertyName("message")]
    [Description("Custom message that will display in the email")]
    public string? Message { get; init; }
}

public class InviteUserActionOutput
{
    [JsonPropertyName("status")]
    [Description("The status of the invitation")]
    public string Status { get; init; } = "Invited";
}
