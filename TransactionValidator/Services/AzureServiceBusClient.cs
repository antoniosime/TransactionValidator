using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionValidator.Interface;
using TransactionValidator.Models;

namespace TransactionValidator.Services
{
    public class AzureServiceBusClient : IAzureServiceBusClient
    {
        private ServiceBusClient _client;
        private ServiceBusSender _sender;
        private readonly ILogger<AzureServiceBusClient> _logger;

        public AzureServiceBusClient
            (
            ServiceBusSender sender,
            IOptions<AzureServiceBusOptions> options,
            ILogger<AzureServiceBusClient> logger
            )
        {
            var connectionString = options.Value.ServiceBusConnectionString;

            _client = new ServiceBusClient(connectionString);
            _sender = sender;
            _logger = logger;
        }

        public async Task Send(ServiceBusMessage message, string queueName)
        {
            await using (_client)
            {
                _sender = _client.CreateSender(queueName);

                try
                {
                    await _sender.SendMessageAsync(message);
                }
                catch (ServiceBusException ex)
                {
                    _logger.LogInformation($"Exception: {ex}. Could not send message to Sevice Bus Queue: {queueName},  for message: {message.MessageId}.");
                }
            }
        }
    }
}
