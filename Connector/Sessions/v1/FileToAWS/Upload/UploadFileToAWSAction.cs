namespace Connector.Sessions.v1.FileToAWS.Upload;

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
[Description("Uploads a file to AWS using a pre-signed URL")]
public class UploadFileToAWSAction : IStandardAction<UploadFileToAWSActionInput, UploadFileToAWSActionOutput>
{
    public UploadFileToAWSActionInput ActionInput { get; set; } = new() { UploadUrl = string.Empty, FileUrl = string.Empty };
    public UploadFileToAWSActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class UploadFileToAWSActionInput
{
    [JsonPropertyName("uploadUrl")]
    [Description("The pre-signed AWS URL to upload the file to")]
    [Required]
    public required string UploadUrl { get; init; }

    [JsonPropertyName("fileUrl")]
    [Description("The URL of the file to upload")]
    [Required]
    public required string FileUrl { get; init; }
}

public class UploadFileToAWSActionOutput
{
    [JsonPropertyName("success")]
    [Description("Whether the upload was successful")]
    public bool Success { get; init; }
}
