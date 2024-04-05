using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionValidator.Interface;
using TransactionValidator.Models.Transaction;

namespace TransactionValidator.Services
{
    public class TransactionValidatorService: ITransactionValidatorService
    {
        private readonly ITenantSettingsService _tenantSettingsService;

        public TransactionValidatorService(ITenantSettingsService tenantSettingsService)
        {
            _tenantSettingsService = tenantSettingsService;
        }

        public Task<bool> Validate(TransactionMessgae transaction)
        {
            var tenantSettings = _tenantSettingsService.GetTenantSettings(transaction.TenantId);

            // Check country sanctions
            if (tenantSettings.CountrySanctions != null)
            {
                // Check if source or destination country is in the sanctioned list
                var sourceCountryCode = transaction.SourceAccount.Countrycode;
                var destinationCountryCode = transaction.SourceAccount.Countrycode;
                if (tenantSettings.CountrySanctions.SourceCountryCode.Contains(sourceCountryCode))
                {
                    return Task.FromResult(false);
                }
                if (tenantSettings.CountrySanctions.DestinationCountryCode.Contains(destinationCountryCode))
                {
                    return Task.FromResult(false);
                }
            }
            // Check thresholds
            if (tenantSettings.Thresholds != null && tenantSettings.Thresholds.PerTransaction != null)
            {
                // Compare transaction amount against per transaction threshold
                if (transaction.Amount > Convert.ToDecimal(tenantSettings.Thresholds.PerTransaction))
                {
                    return Task.FromResult(false);
                }
            }

            // Check velocity limits
            if (tenantSettings.VelocityLimits != null && tenantSettings.VelocityLimits.Daily != null)
            {
                // ToDo: Get this value from the accouunt service 
                var dalayAmount = 0;

                // Compare transaction amount and count against daily velocity limit 
                if (dalayAmount + transaction.Amount > Convert.ToDecimal(tenantSettings.VelocityLimits.Daily))
                {
                    return Task.FromResult(false);
                }
            }

            return Task.FromResult(true);
        }
    }
}
