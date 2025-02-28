namespace Connector.Notifications.v1;
using Connector.Notifications.v1.ComprehensiveSubscription;
using Connector.Notifications.v1.ComprehensiveSubscription.Create;
using Connector.Notifications.v1.Subscription;
using Connector.Notifications.v1.Subscription.Delete;
using Connector.Notifications.v1.Subscription.SubscribeDocument;
using Connector.Notifications.v1.Subscription.SubscribeFileFolder;
using Connector.Notifications.v1.Subscription.SubscribeStudioProject;
using Connector.Notifications.v1.Subscription.SubscribeStudioSession;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.Action;

public class NotificationsV1ActionProcessorServiceDefinition : BaseActionHandlerServiceDefinition<NotificationsV1ActionProcessorConfig>
{
    public override string ModuleId => "notifications-1";
    public override Type ServiceType => typeof(GenericActionHandlerService<NotificationsV1ActionProcessorConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };
        var serviceConfig = JsonSerializer.Deserialize<NotificationsV1ActionProcessorConfig>(serviceConfigJson, options);
        serviceCollection.AddSingleton<NotificationsV1ActionProcessorConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericActionHandlerService<NotificationsV1ActionProcessorConfig>>();
        serviceCollection.AddSingleton<IActionHandlerServiceDefinition<NotificationsV1ActionProcessorConfig>>(this);
        // Register Action Handlers as scoped dependencies
        serviceCollection.AddScoped<SubscribeStudioProjectSubscriptionHandler>();
        serviceCollection.AddScoped<SubscribeFileFolderSubscriptionHandler>();
        serviceCollection.AddScoped<SubscribeStudioSessionSubscriptionHandler>();
        serviceCollection.AddScoped<SubscribeDocumentSubscriptionHandler>();
        serviceCollection.AddScoped<CreateComprehensiveSubscriptionHandler>();
        serviceCollection.AddScoped<DeleteSubscriptionHandler>();
    }

    public override void ConfigureService(IActionHandlerService service, NotificationsV1ActionProcessorConfig config)
    {
        // Register Action Handler configurations for the Action Processor Service
        service.RegisterHandlerForDataObjectAction<SubscribeStudioProjectSubscriptionHandler, SubscriptionDataObject>(ModuleId, "subscription", "subscribe-studio-project", config.SubscribeStudioProjectSubscriptionConfig);
        service.RegisterHandlerForDataObjectAction<SubscribeFileFolderSubscriptionHandler, SubscriptionDataObject>(ModuleId, "subscription", "subscribe-file-folder", config.SubscribeFileFolderSubscriptionConfig);
        service.RegisterHandlerForDataObjectAction<SubscribeStudioSessionSubscriptionHandler, SubscriptionDataObject>(ModuleId, "subscription", "subscribe-studio-session", config.SubscribeStudioSessionSubscriptionConfig);
        service.RegisterHandlerForDataObjectAction<SubscribeDocumentSubscriptionHandler, SubscriptionDataObject>(ModuleId, "subscription", "subscribe-document", config.SubscribeDocumentSubscriptionConfig);
        service.RegisterHandlerForDataObjectAction<CreateComprehensiveSubscriptionHandler, ComprehensiveSubscriptionDataObject>(ModuleId, "comprehensive-subscription", "create", config.CreateComprehensiveSubscriptionConfig);
        service.RegisterHandlerForDataObjectAction<DeleteSubscriptionHandler, SubscriptionDataObject>(ModuleId, "subscription", "delete", config.DeleteSubscriptionConfig);
    }
}