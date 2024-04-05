using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionValidator.Models.Transaction
{
    public class DestinationAccount
    {
        public string Accountno { get; set; }
        public string Sortcode { get; set; }
        public string Countrycode { get; set; }
    }
}
