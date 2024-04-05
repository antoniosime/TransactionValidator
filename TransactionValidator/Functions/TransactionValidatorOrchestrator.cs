using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TransactionValidator.Interface;
using TransactionValidator.Models.Transaction;

namespace TransactionValidator.Functions
{
    public  class TransactionValidatorOrchestrator
    {
        private readonly ITransactionValidatorService _transactionValidatorService;
        private readonly IAzureServiceBusClient _client;

        public TransactionValidatorOrchestrator(ITransactionValidatorService transactionValidatorService, IAzureServiceBusClient client)
        {
            _transactionValidatorService = transactionValidatorService;
            _client = client;

        }

        [FunctionName("TransactionValidatorOrchestrator")]
        public  async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var message = context.GetInput<TransactionMessgae>();
            try
            {
                var isValidTransaction = await context.CallActivityAsync<bool>(nameof(ValidateTransaction), message);

                if (isValidTransaction)
                {
                    await context.CallActivityAsync<bool>(nameof(ProcessingPayment), message);
                }
                else
                {
                    await context.CallActivityAsync<bool>(nameof(HoldPayment), message);
                }
            }
            catch(Exception ex)
            {
                await context.CallActivityAsync<bool>(nameof(ReportErrorPayment), message);
            }
            
        }

        [FunctionName(nameof(ValidateTransaction))]
        public  async Task<bool> ValidateTransaction([ActivityTrigger] TransactionMessgae transaction, ILogger log)
        {
            return await _transactionValidatorService.Validate(transaction);
        }

        [FunctionName(nameof(ProcessingPayment))]
        public async Task<bool> ProcessingPayment([ActivityTrigger] TransactionMessgae message, ILogger log)
        {
            await _client.Send(new ServiceBusMessage(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)))
            {
                MessageId = message.TransactionId
            }, "processing-payment");

            return true;
        }

        [FunctionName(nameof(HoldPayment))]
        public async Task<bool> HoldPayment([ActivityTrigger] TransactionMessgae message, ILogger log)
        {
            await _client.Send(new ServiceBusMessage(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)))
            {
                MessageId = message.TransactionId
            }, "holding-payment");

            return true;
        }

        [FunctionName(nameof(ReportErrorPayment))]
        public async Task<bool> ReportErrorPayment([ActivityTrigger] TransactionMessgae message, ILogger log)
        {
            await _client.Send(new ServiceBusMessage(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)))
            {
                MessageId = message.TransactionId
            }, "operations-payment");

            return true;
        }

    }
}