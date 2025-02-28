using System;
using ESR.Hosting;
using Microsoft.Extensions.Hosting;
using Xchange.Connector.SDK.Hosting;
using Xchange.Connector.SDK.Test.Local;

namespace Connector;

/// <summary>
/// Main executable entry point for the connector.
/// </summary>
public static class Program
{
    public static void Main(string[] args)
    {
        // DO NOT MODIFY: This check is to make sure that the connector has the proper environment variable available to it to function.
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ESR__QueueName")))
        {
            Console.Error.WriteLine("Exiting 'ESR__QueueName' environment variable not provided");
            Environment.Exit(1);
        }
        
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureLocalDevelopment(args)
            .UseGenericServiceRun<ServiceRunner>(new ConnectorRegistration(), args)
            .Build();

        host.Run();
    }
}