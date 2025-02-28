namespace Connector.Notifications.v1;
using Connector.Notifications.v1.ComprehensiveSubscription;
using Connector.Notifications.v1.Subscription;
using Connector.Notifications.v1.Subscriptions;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using Xchange.Connector.SDK.Abstraction.Change;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.CacheWriter;
using Xchange.Connector.SDK.Hosting.Configuration;

public class NotificationsV1CacheWriterServiceDefinition : BaseCacheWriterServiceDefinition<NotificationsV1CacheWriterConfig>
{
    public override string ModuleId => "notifications-1";
    public override Type ServiceType => typeof(GenericCacheWriterService<NotificationsV1CacheWriterConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var serviceConfig = JsonSerializer.Deserialize<NotificationsV1CacheWriterConfig>(serviceConfigJson);
        serviceCollection.AddSingleton<NotificationsV1CacheWriterConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericCacheWriterService<NotificationsV1CacheWriterConfig>>();
        serviceCollection.AddSingleton<ICacheWriterServiceDefinition<NotificationsV1CacheWriterConfig>>(this);
        // Register Data Readers as Singletons
        serviceCollection.AddSingleton<SubscriptionsDataReader>();
        serviceCollection.AddSingleton<SubscriptionDataReader>();
        serviceCollection.AddSingleton<ComprehensiveSubscriptionDataReader>();
    }

    public override IDataObjectChangeDetectorProvider ConfigureChangeDetectorProvider(IChangeDetectorFactory factory, ConnectorDefinition connectorDefinition)
    {
        var options = factory.CreateProviderOptionsWithNoDefaultResolver();
        // Configure Data Object Keys for Data Objects that do not use the default
        this.RegisterKeysForObject<SubscriptionsDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<SubscriptionDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ComprehensiveSubscriptionDataObject>(options, connectorDefinition);
        return factory.CreateProvider(options);
    }

    public override void ConfigureService(ICacheWriterService service, NotificationsV1CacheWriterConfig config)
    {
        var dataReaderSettings = new DataReaderSettings
        {
            DisableDeletes = false,
            UseChangeDetection = true
        };
        // Register Data Reader configurations for the Cache Writer Service
        service.RegisterDataReader<SubscriptionsDataReader, SubscriptionsDataObject>(ModuleId, config.SubscriptionsConfig, dataReaderSettings);
        service.RegisterDataReader<SubscriptionDataReader, SubscriptionDataObject>(ModuleId, config.SubscriptionConfig, dataReaderSettings);
        service.RegisterDataReader<ComprehensiveSubscriptionDataReader, ComprehensiveSubscriptionDataObject>(ModuleId, config.ComprehensiveSubscriptionConfig, dataReaderSettings);
    }
}