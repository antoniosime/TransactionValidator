using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionValidator.Models.Transaction;

namespace TransactionValidator.Interface
{
    public interface ITransactionValidatorService
    {
        Task<bool> Validate(TransactionMessgae transaction);
    }
}
