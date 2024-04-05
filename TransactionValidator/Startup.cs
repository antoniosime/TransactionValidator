using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionValidator.Interface;
using TransactionValidator.Models;
using TransactionValidator.Services;


[assembly: FunctionsStartup(typeof(TransactionValidator.Startup))]

namespace TransactionValidator
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

            // #1 Register Configuration as a singleton
            builder.Services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", true, true)
            .AddEnvironmentVariables()
            .Build());

            // #2 Build Service Provider
            var serviceProvider = builder.Services.BuildServiceProvider();

            // #3 use configuration to inject settings (e.g. local.settings.json)
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            builder.Services.Configure<AzureServiceBusOptions>(options =>
            {
                options.ServiceBusConnectionString = configuration["AzureServiceBus"];
            });

            builder.Services.AddSingleton<IAzureServiceBusClient, AzureServiceBusClient>();


            builder.Services.AddTransient<ITenantSettingsService, TenantSettingsService>();
            builder.Services.AddTransient<ITransactionValidatorService, TransactionValidatorService>();

        }
    }
}
