using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionValidator.Interface;
using TransactionValidator.Models;
using TransactionValidator.Models.Settings;

namespace TransactionValidator.Services
{
    public class TenantSettingsService : ITenantSettingsService
    {
        private List<TenantSettings> _tenantSettings;

        public TenantSettingsService()
        {
            _tenantSettings = new List<TenantSettings>
            {
                new TenantSettings
                {
                    Tenantid="345",
                    VelocityLimits= new VelocityLimits
                    {
                        Daily="2500"
                    },
                    Thresholds= new Thresholds
                    {
                        PerTransaction="1500"
                    },
                    CountrySanctions= new CountrySanctions
                    {
                        SourceCountryCode= "AFG, BLR, BIH, IRQ, KEN, RUS",
                        DestinationCountryCode="AFG, BLR, BIH, IRQ, KEN, RUS, TKM, UGA"
                    }
                }
            };
        }

        public TenantSettings GetTenantSettings(string tenantId)
        {
            return _tenantSettings.FirstOrDefault(x=>x.Tenantid == tenantId);
        }
    }
}
