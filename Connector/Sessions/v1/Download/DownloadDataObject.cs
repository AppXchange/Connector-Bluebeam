namespace Connector.Sessions.v1.Download;

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
[PrimaryKey("sessionId", nameof(SessionId), "fileId", nameof(FileId))]
//[AlternateKey("alt-key-id", nameof(CompanyId), nameof(EquipmentNumber))]
[Description("Represents a downloadable snapshot of a file in a Bluebeam Studio Session")]
public class DownloadDataObject
{
    [JsonPropertyName("sessionId")]
    [Description("The ID of the session containing the file")]
    [Required]
    public required string SessionId { get; init; }

    [JsonPropertyName("fileId")]
    [Description("The ID of the file to download")]
    [Required]
    public required int FileId { get; init; }

    [JsonPropertyName("status")]
    [Description("The current status of the snapshot")]
    [Required]
    public required string Status { get; init; }

    [JsonPropertyName("statusTime")]
    [Description("The timestamp when the status was last updated")]
    public DateTime StatusTime { get; init; }

    [JsonPropertyName("lastSnapshotTime")]
    [Description("The timestamp of the last successful snapshot")]
    public DateTime? LastSnapshotTime { get; init; }

    [JsonPropertyName("downloadUrl")]
    [Description("The URL to download the snapshot when status is Complete")]
    public string? DownloadUrl { get; init; }

    [JsonPropertyName("errorMessage")]
    [Description("Error message if status is Error")]
    public string? ErrorMessage { get; init; }
}