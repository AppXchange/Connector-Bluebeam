using Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.Sessions.v1.Session;

[PrimaryKey("id", nameof(Id))]
[Description("Represents a Bluebeam Studio Session")]
public class SessionDataObject
{
    [JsonPropertyName("id")]
    [Description("The unique identifier for the session")]
    [Required]
    public required string Id { get; init; }

    [JsonPropertyName("name")]
    [Description("The name of the session")]
    [Required]
    public required string Name { get; init; }

    [JsonPropertyName("notification")]
    [Description("Whether the current user will be subscribed to email notifications")]
    public bool Notification { get; init; }

    [JsonPropertyName("restricted")]
    [Description("Whether the session is restricted to invited users only")]
    public bool Restricted { get; init; }

    [JsonPropertyName("sessionEndDate")]
    [Description("The date when the session will end")]
    public DateTime? SessionEndDate { get; init; }

    [JsonPropertyName("defaultPermissions")]
    [Description("Default permissions for session attendees")]
    public List<SessionPermission> DefaultPermissions { get; init; } = new();
}

public class SessionPermission
{
    [JsonPropertyName("type")]
    [Description("The type of permission")]
    public required string Type { get; init; }

    [JsonPropertyName("allow")]
    [Description("The permission state (Allow, Deny, Default)")]
    public required string Allow { get; init; }
}