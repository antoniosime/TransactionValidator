using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionValidator.Models.Settings
{
    public class TenantSettings
    {
        public string Tenantid { get; set; }
        public VelocityLimits VelocityLimits { get; set; }
        public Thresholds Thresholds { get; set; }
        public CountrySanctions CountrySanctions { get; set; }
    }
}
