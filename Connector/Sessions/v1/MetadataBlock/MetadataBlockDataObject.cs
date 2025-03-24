namespace Connector.Sessions.v1.MetadataBlock;

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
[PrimaryKey("id", nameof(Id))]
//[AlternateKey("alt-key-id", nameof(CompanyId), nameof(EquipmentNumber))]
[Description("Represents a Bluebeam Studio Session file metadata block")]
public class MetadataBlockDataObject
{
    [JsonPropertyName("id")]
    [Description("The unique identifier for the metadata block")]
    [Required]
    public required int Id { get; init; }

    [JsonPropertyName("name")]
    [Description("Name of the file (must end in .pdf)")]
    [Required]
    public required string Name { get; init; }

    [JsonPropertyName("source")]
    [Description("Source path of the file")]
    public string? Source { get; init; }

    [JsonPropertyName("size")]
    [Description("File size (optional, server will calculate if null)")]
    public int? Size { get; init; }

    [JsonPropertyName("crc")]
    [Description("CRC value (optional, server will calculate if null)")]
    public string? CRC { get; init; }

    [JsonPropertyName("uploadUrl")]
    [Description("URL for uploading the file")]
    public string? UploadUrl { get; init; }

    [JsonPropertyName("uploadContentType")]
    [Description("Content type for the upload")]
    public string? UploadContentType { get; init; }
}