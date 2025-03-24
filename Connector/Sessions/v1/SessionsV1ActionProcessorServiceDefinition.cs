namespace Connector.Sessions.v1;
using Connector.Sessions.v1.FileToAWS;
using Connector.Sessions.v1.FileToAWS.Upload;
using Connector.Sessions.v1.Finalizing;
using Connector.Sessions.v1.Finalizing.SetStatus;
using Connector.Sessions.v1.MetadataBlock;
using Connector.Sessions.v1.MetadataBlock.Create;
using Connector.Sessions.v1.Session;
using Connector.Sessions.v1.Session.Delete;
using Connector.Sessions.v1.Session.Initialize;
using Connector.Sessions.v1.Snapshop;
using Connector.Sessions.v1.Snapshop.Create;
using Connector.Sessions.v1.User;
using Connector.Sessions.v1.User.Add;
using Connector.Sessions.v1.User.Invite;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.Action;

public class SessionsV1ActionProcessorServiceDefinition : BaseActionHandlerServiceDefinition<SessionsV1ActionProcessorConfig>
{
    public override string ModuleId => "sessions-1";
    public override Type ServiceType => typeof(GenericActionHandlerService<SessionsV1ActionProcessorConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };
        var serviceConfig = JsonSerializer.Deserialize<SessionsV1ActionProcessorConfig>(serviceConfigJson, options);
        serviceCollection.AddSingleton<SessionsV1ActionProcessorConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericActionHandlerService<SessionsV1ActionProcessorConfig>>();
        serviceCollection.AddSingleton<IActionHandlerServiceDefinition<SessionsV1ActionProcessorConfig>>(this);
        // Register Action Handlers as scoped dependencies
        serviceCollection.AddScoped<InitializeSessionHandler>();
        serviceCollection.AddScoped<CreateMetadataBlockHandler>();
        serviceCollection.AddScoped<UploadFileToAWSHandler>();
        serviceCollection.AddScoped<InviteUserHandler>();
        serviceCollection.AddScoped<SetStatusFinalizingHandler>();
        serviceCollection.AddScoped<CreateSnapshopHandler>();
        serviceCollection.AddScoped<DeleteSessionHandler>();
        serviceCollection.AddScoped<AddUserHandler>();
    }

    public override void ConfigureService(IActionHandlerService service, SessionsV1ActionProcessorConfig config)
    {
        // Register Action Handler configurations for the Action Processor Service
        service.RegisterHandlerForDataObjectAction<InitializeSessionHandler, SessionDataObject>(ModuleId, "session", "initialize", config.InitializeSessionConfig);
        service.RegisterHandlerForDataObjectAction<CreateMetadataBlockHandler, MetadataBlockDataObject>(ModuleId, "metadata-block", "create", config.CreateMetadataBlockConfig);
        service.RegisterHandlerForDataObjectAction<UploadFileToAWSHandler, FileToAWSDataObject>(ModuleId, "file-to-aws", "upload", config.UploadFileToAWSConfig);
        service.RegisterHandlerForDataObjectAction<InviteUserHandler, UserDataObject>(ModuleId, "user", "invite", config.InviteUserConfig);
        service.RegisterHandlerForDataObjectAction<SetStatusFinalizingHandler, FinalizingDataObject>(ModuleId, "finalizing", "set-status", config.SetStatusFinalizingConfig);
        service.RegisterHandlerForDataObjectAction<CreateSnapshopHandler, SnapshopDataObject>(ModuleId, "snapshop", "create", config.CreateSnapshopConfig);
        service.RegisterHandlerForDataObjectAction<DeleteSessionHandler, SessionDataObject>(ModuleId, "session", "delete", config.DeleteSessionConfig);
        service.RegisterHandlerForDataObjectAction<AddUserHandler, UserDataObject>(ModuleId, "user", "add", config.AddUserConfig);
    }
}