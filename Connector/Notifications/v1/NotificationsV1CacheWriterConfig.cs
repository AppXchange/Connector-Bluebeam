namespace Connector.Notifications.v1;
using Connector.Notifications.v1.ComprehensiveSubscription;
using Connector.Notifications.v1.Subscription;
using Connector.Notifications.v1.Subscriptions;
using ESR.Hosting.CacheWriter;
using Json.Schema.Generation;

/// <summary>
/// Configuration for the Cache writer for this module. This configuration will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// The schema will be used for validation at runtime to make sure the configurations are properly formed. 
/// The schema also helps provide integrators more information for what the values are intended to be.
/// </summary>
[Title("Notifications V1 Cache Writer Configuration")]
[Description("Configuration of the data object caches for the module.")]
public class NotificationsV1CacheWriterConfig
{
    // Data Reader configuration
    public CacheWriterObjectConfig SubscriptionsConfig { get; set; } = new();
    public CacheWriterObjectConfig SubscriptionConfig { get; set; } = new();
    public CacheWriterObjectConfig ComprehensiveSubscriptionConfig { get; set; } = new();
}