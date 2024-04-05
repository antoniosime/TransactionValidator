using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionValidator.Models.Settings
{
    public class CountrySanctions
    {
        public string SourceCountryCode { get; set; }
        public string DestinationCountryCode { get; set; }
    }
}
