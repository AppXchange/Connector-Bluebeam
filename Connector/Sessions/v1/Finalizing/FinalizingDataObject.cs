namespace Connector.Sessions.v1.Finalizing;

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
[PrimaryKey("sessionId", nameof(SessionId))]
[Description("Represents a Bluebeam Studio Session's finalizing status")]
public class FinalizingDataObject
{
    [JsonPropertyName("sessionId")]
    [Description("The ID of the session being finalized")]
    [Required]
    public required string SessionId { get; init; }

    [JsonPropertyName("status")]
    [Description("The status of the session")]
    [Required]
    public required string Status { get; init; }

    [JsonPropertyName("finalizingTime")]
    [Description("The timestamp when the session was set to finalizing status")]
    public DateTime FinalizingTime { get; init; }
}