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
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using Xchange.Connector.SDK.Abstraction.Change;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.CacheWriter;
using Xchange.Connector.SDK.Hosting.Configuration;

public class SessionsV1CacheWriterServiceDefinition : BaseCacheWriterServiceDefinition<SessionsV1CacheWriterConfig>
{
    public override string ModuleId => "sessions-1";
    public override Type ServiceType => typeof(GenericCacheWriterService<SessionsV1CacheWriterConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var serviceConfig = JsonSerializer.Deserialize<SessionsV1CacheWriterConfig>(serviceConfigJson);
        serviceCollection.AddSingleton<SessionsV1CacheWriterConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericCacheWriterService<SessionsV1CacheWriterConfig>>();
        serviceCollection.AddSingleton<ICacheWriterServiceDefinition<SessionsV1CacheWriterConfig>>(this);
        // Register Data Readers as Singletons
        serviceCollection.AddSingleton<SessionDataReader>();
        serviceCollection.AddSingleton<MetadataBlockDataReader>();
        serviceCollection.AddSingleton<FileToAWSDataReader>();
        serviceCollection.AddSingleton<ConfirmUploadDataReader>();
        serviceCollection.AddSingleton<UserDataReader>();
        serviceCollection.AddSingleton<FinalizingDataReader>();
        serviceCollection.AddSingleton<SnapshopDataReader>();
        serviceCollection.AddSingleton<DownloadDataReader>();
    }

    public override IDataObjectChangeDetectorProvider ConfigureChangeDetectorProvider(IChangeDetectorFactory factory, ConnectorDefinition connectorDefinition)
    {
        var options = factory.CreateProviderOptionsWithNoDefaultResolver();
        // Configure Data Object Keys for Data Objects that do not use the default
        this.RegisterKeysForObject<SessionDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<MetadataBlockDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<FileToAWSDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ConfirmUploadDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<UserDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<FinalizingDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<SnapshopDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<DownloadDataObject>(options, connectorDefinition);
        return factory.CreateProvider(options);
    }

    public override void ConfigureService(ICacheWriterService service, SessionsV1CacheWriterConfig config)
    {
        var dataReaderSettings = new DataReaderSettings
        {
            DisableDeletes = false,
            UseChangeDetection = true
        };
        // Register Data Reader configurations for the Cache Writer Service
        service.RegisterDataReader<SessionDataReader, SessionDataObject>(ModuleId, config.SessionConfig, dataReaderSettings);
        service.RegisterDataReader<MetadataBlockDataReader, MetadataBlockDataObject>(ModuleId, config.MetadataBlockConfig, dataReaderSettings);
        service.RegisterDataReader<FileToAWSDataReader, FileToAWSDataObject>(ModuleId, config.FileToAWSConfig, dataReaderSettings);
        service.RegisterDataReader<ConfirmUploadDataReader, ConfirmUploadDataObject>(ModuleId, config.ConfirmUploadConfig, dataReaderSettings);
        service.RegisterDataReader<UserDataReader, UserDataObject>(ModuleId, config.UserConfig, dataReaderSettings);
        service.RegisterDataReader<FinalizingDataReader, FinalizingDataObject>(ModuleId, config.FinalizingConfig, dataReaderSettings);
        service.RegisterDataReader<SnapshopDataReader, SnapshopDataObject>(ModuleId, config.SnapshopConfig, dataReaderSettings);
        service.RegisterDataReader<DownloadDataReader, DownloadDataObject>(ModuleId, config.DownloadConfig, dataReaderSettings);
    }
}