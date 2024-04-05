using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionValidator.Models.Transaction
{
    public class TransactionMessgae
    {
        public string CorrelationId { get; set; }
        public string TenantId { get; set; }
        public string TransactionId { get; set; }
        public string TransactionDate { get; set; }
        public string Direction { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public SourceAccount SourceAccount { get; set; }
        public DestinationAccount DestinationAccount { get; set; }
    }
}
