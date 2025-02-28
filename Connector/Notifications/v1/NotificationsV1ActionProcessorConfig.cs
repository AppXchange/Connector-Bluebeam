namespace Connector.Notifications.v1;
using Connector.Notifications.v1.ComprehensiveSubscription.Create;
using Connector.Notifications.v1.Subscription.Delete;
using Connector.Notifications.v1.Subscription.SubscribeDocument;
using Connector.Notifications.v1.Subscription.SubscribeFileFolder;
using Connector.Notifications.v1.Subscription.SubscribeStudioProject;
using Connector.Notifications.v1.Subscription.SubscribeStudioSession;
using Json.Schema.Generation;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Configuration for the Action Processor for this module. This configuration will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// The schema will be used for validation at runtime to make sure the configurations are properly formed. 
/// The schema also helps provide integrators more information for what the values are intended to be.
/// </summary>
[Title("Notifications V1 Action Processor Configuration")]
[Description("Configuration of the data object actions for the module.")]
public class NotificationsV1ActionProcessorConfig
{
    // Action Handler configuration
    public DefaultActionHandlerConfig SubscribeStudioProjectSubscriptionConfig { get; set; } = new();
    public DefaultActionHandlerConfig SubscribeFileFolderSubscriptionConfig { get; set; } = new();
    public DefaultActionHandlerConfig SubscribeStudioSessionSubscriptionConfig { get; set; } = new();
    public DefaultActionHandlerConfig SubscribeDocumentSubscriptionConfig { get; set; } = new();
    public DefaultActionHandlerConfig CreateComprehensiveSubscriptionConfig { get; set; } = new();
    public DefaultActionHandlerConfig DeleteSubscriptionConfig { get; set; } = new();
}