namespace Connector.Sessions.v1.ConfirmUpload;

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
[Description("Represents a confirmed file upload in a Bluebeam Studio Session")]
public class ConfirmUploadDataObject
{
    [JsonPropertyName("id")]
    [Description("The unique identifier of the file")]
    [Required]
    public required int Id { get; init; }

    [JsonPropertyName("sessionId")]
    [Description("The ID of the session containing the file")]
    [Required]
    public required string SessionId { get; init; }

    [JsonPropertyName("status")]
    [Description("The status of the file upload confirmation")]
    public string? Status { get; init; }

    [JsonPropertyName("confirmationTime")]
    [Description("The timestamp when the upload was confirmed")]
    public DateTime? ConfirmationTime { get; init; }
}