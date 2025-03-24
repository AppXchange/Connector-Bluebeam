using Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

namespace Connector.Sessions.v1.Session.Initialize;

[Description("Initializes a new Bluebeam Studio Session")]
public class InitializeSessionAction : IStandardAction<InitializeSessionActionInput, InitializeSessionActionOutput>
{
    public InitializeSessionActionInput ActionInput { get; set; } = new() { Name = string.Empty };
    public InitializeSessionActionOutput ActionOutput { get; set; } = new() { Id = string.Empty };
    public StandardActionFailure ActionFailure { get; set; } = new();
    public bool CreateRtap => true;
}

public class InitializeSessionActionInput
{
    [JsonPropertyName("name")]
    [Description("The name of the session")]
    [Required]
    public required string Name { get; init; }

    [JsonPropertyName("notification")]
    [Description("Whether to subscribe to email notifications")]
    public bool Notification { get; init; }

    [JsonPropertyName("restricted")]
    [Description("Whether to restrict the session to invited users only")]
    public bool Restricted { get; init; }

    [JsonPropertyName("sessionEndDate")]
    [Description("The date when the session will end (UTC)")]
    public DateTime? SessionEndDate { get; init; }

    [JsonPropertyName("defaultPermissions")]
    [Description("Default permissions for session attendees")]
    public List<SessionPermission> DefaultPermissions { get; init; } = new();
}

public class InitializeSessionActionOutput
{
    [JsonPropertyName("id")]
    [Description("The unique identifier of the created session")]
    public required string Id { get; init; }
}