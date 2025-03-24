namespace Connector.Sessions.v1.User;

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
[PrimaryKey("email", nameof(Email))]
//[AlternateKey("alt-key-id", nameof(CompanyId), nameof(EquipmentNumber))]
[Description("Represents a user in a Bluebeam Studio Session")]
public class UserDataObject
{
    [JsonPropertyName("email")]
    [Description("The email address of the user")]
    [Required]
    public required string Email { get; init; }

    [JsonPropertyName("sessionId")]
    [Description("The ID of the session the user is invited to")]
    [Required]
    public required string SessionId { get; init; }

    [JsonPropertyName("invitationStatus")]
    [Description("The status of the user's invitation")]
    public string? InvitationStatus { get; init; }

    [JsonPropertyName("invitationTime")]
    [Description("The timestamp when the user was invited")]
    public DateTime? InvitationTime { get; init; }

    [JsonPropertyName("message")]
    [Description("The invitation message sent to the user")]
    public string? Message { get; init; }
}