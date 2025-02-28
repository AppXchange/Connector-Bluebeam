namespace Connector.Sessions.v1;
using Connector.Sessions.v1.FileToAWS.Upload;
using Connector.Sessions.v1.Finalizing.SetStatus;
using Connector.Sessions.v1.MetadataBlock.Create;
using Connector.Sessions.v1.Session.Delete;
using Connector.Sessions.v1.Session.Initialize;
using Connector.Sessions.v1.Snapshop.Create;
using Connector.Sessions.v1.User.Invite;
using Json.Schema.Generation;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Configuration for the Action Processor for this module. This configuration will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// The schema will be used for validation at runtime to make sure the configurations are properly formed. 
/// The schema also helps provide integrators more information for what the values are intended to be.
/// </summary>
[Title("Sessions V1 Action Processor Configuration")]
[Description("Configuration of the data object actions for the module.")]
public class SessionsV1ActionProcessorConfig
{
    // Action Handler configuration
    public DefaultActionHandlerConfig InitializeSessionConfig { get; set; } = new();
    public DefaultActionHandlerConfig CreateMetadataBlockConfig { get; set; } = new();
    public DefaultActionHandlerConfig UploadFileToAWSConfig { get; set; } = new();
    public DefaultActionHandlerConfig InviteUserConfig { get; set; } = new();
    public DefaultActionHandlerConfig SetStatusFinalizingConfig { get; set; } = new();
    public DefaultActionHandlerConfig CreateSnapshopConfig { get; set; } = new();
    public DefaultActionHandlerConfig DeleteSessionConfig { get; set; } = new();
}