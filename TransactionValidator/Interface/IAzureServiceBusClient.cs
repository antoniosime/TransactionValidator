using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionValidator.Interface
{
    public interface IAzureServiceBusClient
    {
        public Task Send(ServiceBusMessage message, string queueName);
    }
}
