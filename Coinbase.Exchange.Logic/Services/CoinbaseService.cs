using Coinbase.Exchange.Domain.Constants;
using Coinbase.Exchange.Domain.Models.Account;
using Coinbase.Exchange.Logic.Builders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Logic.Services
{
    public class CoinbaseService : ICoinbaseService
    {
        private readonly IRequestBuilder _requestBuilder;
        private readonly ILogger<CoinbaseService> _logger;

        public CoinbaseService(IRequestBuilder requestBuilder, 
            ILogger<CoinbaseService> logger)
        {
            _requestBuilder = requestBuilder;
            _logger = logger;
        }

        public async Task<AccountEntry?> GetAccountEntryAsync(string uuid)
        {
            var account = await _requestBuilder
                                        .AddResource(Resources.ACCOUNTS)
                                        .AddResource(uuid)
                                        .GetAsync<AccountEntry>();
            return account;
        }

        public async Task<Account?> GetAllAccountAsync(string limit, string cursor)
        {
            var accounts = await _requestBuilder
                                        .AddResource(Resources.ACCOUNTS)
                                        .AddQueryParameters(GetQueryParameters(limit,cursor))
                                        .GetAsync<Account>();
            return accounts;
        }

        public async Task<Domain.Models.Products.Instrument?> GetInstrumentsAsync(string limit, string offset,
            string product_type, string product_ids, string contract_expiry_date)
        {
            var instruments = await _requestBuilder
                                        .AddResource(Resources.PRODUCTS)
                                        .AddQueryParameters(GetQueryParameters(limit,offset,product_type,
                                                            product_ids,contract_expiry_date))
                                        .GetAsync<Domain.Models.Products.Instrument?>();

            return instruments;
        }

        private Dictionary<string,string> GetQueryParameters(params string[] parameters)
        {
            var queries = new Dictionary<string, string>();

            foreach (var parameter in parameters)
            {
                if (!string.IsNullOrEmpty(parameter))
                {
                    queries[nameof(parameter)] = parameter;
                }
            }

            return queries;
        }
    }
}
