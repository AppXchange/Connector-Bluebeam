namespace Connector.Sessions.v1;
using Connector.Sessions.v1.ConfirmUpload;
using Connector.Sessions.v1.Download;
using Connector.Sessions.v1.FileToAWS;
using Connector.Sessions.v1.Finalizing;
using Connector.Sessions.v1.MetadataBlock;
using Connector.Sessions.v1.Session;
using Connector.Sessions.v1.Snapshop;
using Connector.Sessions.v1.User;
using ESR.Hosting.CacheWriter;
using Json.Schema.Generation;

/// <summary>
/// Configuration for the Cache writer for this module. This configuration will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// The schema will be used for validation at runtime to make sure the configurations are properly formed. 
/// The schema also helps provide integrators more information for what the values are intended to be.
/// </summary>
[Title("Sessions V1 Cache Writer Configuration")]
[Description("Configuration of the data object caches for the module.")]
public class SessionsV1CacheWriterConfig
{
    // Data Reader configuration
    public CacheWriterObjectConfig SessionConfig { get; set; } = new();
    public CacheWriterObjectConfig MetadataBlockConfig { get; set; } = new();
    public CacheWriterObjectConfig FileToAWSConfig { get; set; } = new();
    public CacheWriterObjectConfig ConfirmUploadConfig { get; set; } = new();
    public CacheWriterObjectConfig UserConfig { get; set; } = new();
    public CacheWriterObjectConfig FinalizingConfig { get; set; } = new();
    public CacheWriterObjectConfig SnapshopConfig { get; set; } = new();
    public CacheWriterObjectConfig DownloadConfig { get; set; } = new();
}