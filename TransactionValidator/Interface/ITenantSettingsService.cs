using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionValidator.Models.Settings;

namespace TransactionValidator.Interface
{
    public interface ITenantSettingsService
    {
        TenantSettings GetTenantSettings(string tenantId);
    }
}
