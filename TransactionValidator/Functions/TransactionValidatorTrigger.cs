using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TransactionValidator.Models.Transaction;

namespace TransactionValidator.Functions
{
    public class TransactionValidatorTrigger
    {
        [FunctionName("TransactionValidatorTrigger")]
        public async Task Run([ServiceBusTrigger(
            topicName: "validate-transaction",
            subscriptionName: "validate-transaction-sub",
            Connection = "AzureServiceBus")]
        TransactionMessgae message,
          [DurableClient] IDurableClient context,
            ServiceBusClient client, ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {message.TransactionId}");
            try
            {
                var instanceId = $"{message.TransactionId}";
                await StartInstance(context, message, instanceId, log);
            }
            catch (Exception ex)
            {
                var error = $"TransactionValidatorTrigger failed: {ex.Message}";
                log.LogError(error);
            }
        }

        private static async Task StartInstance(IDurableOrchestrationClient context, TransactionMessgae messgae, string instanceId, ILogger log)
        {
            try
            {
                var reportStatus = await context.GetStatusAsync(instanceId);
                string runningStatus = reportStatus == null ? "NULL" : reportStatus.RuntimeStatus.ToString();
                log.LogInformation($"Instance running status: '{runningStatus}'.");

                if (reportStatus == null || reportStatus.RuntimeStatus != OrchestrationRuntimeStatus.Running)
                {
                    await context.StartNewAsync(nameof(TransactionValidatorOrchestrator), instanceId, messgae);
                    log.LogInformation($"Validate transaction = '{instanceId}'.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
