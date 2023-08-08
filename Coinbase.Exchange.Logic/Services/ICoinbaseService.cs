using Coinbase.Exchange.Domain.Models.Account;
using Coinbase.Exchange.Domain.Models.Products;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Logic.Services
{
    public interface ICoinbaseService
    {
        Task<Domain.Models.Products.Instrument?> GetInstrumentsAsync(string limit,string offset, 
            string product_type, string prodduct_ids, string contract_expiry_date);
        Task<Account?> GetAllAccountAsync(string limit, string cursor);
        Task<AccountEntry?> GetAccountEntryAsync(string uuid);

        
    }
}
