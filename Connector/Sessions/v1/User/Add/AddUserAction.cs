namespace Connector.Sessions.v1.User.Add;

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
[Description("Adds a user to a Bluebeam Studio Session")]
public class AddUserAction : IStandardAction<AddUserActionInput, AddUserActionOutput>
{
    public AddUserActionInput ActionInput { get; set; } = new() { Email = string.Empty, SessionId = string.Empty };
    public AddUserActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class AddUserActionInput
{
    [JsonPropertyName("sessionId")]
    [Description("The ID of the session to add the user to")]
    [Required]
    public required string SessionId { get; init; }

    [JsonPropertyName("email")]
    [Description("Email address of known Studio account")]
    [Required]
    public required string Email { get; init; }

    [JsonPropertyName("sendEmail")]
    [Description("Whether to send an email notification to the user")]
    public bool SendEmail { get; init; }

    [JsonPropertyName("message")]
    [Description("Custom message that will display in the email, if the email is sent")]
    public string? Message { get; init; }
}

public class AddUserActionOutput
{
    [JsonPropertyName("status")]
    [Description("The status of adding the user")]
    public string Status { get; init; } = "Added";
}
