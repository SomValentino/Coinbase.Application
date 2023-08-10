using Coinbase.Exchange.Logic.Builders;
using Coinbase.Exchange.SharedKernel.Constants;
using Coinbase.Exchange.SharedKernel.Models.Account;
using Coinbase.Exchange.SharedKernel.Models.Bids;
using Coinbase.Exchange.SharedKernel.Models.Order;
using Coinbase.Exchange.SharedKernel.Models.Products;
using Microsoft.Extensions.Logging;

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

        public async Task<Account?> GetAllAccountAsync(params string[] query)
        {
            var accounts = await _requestBuilder
                                        .AddResource(Resources.ACCOUNTS)
                                        .AddQueryParameters(GetQueryParameters(query))
                                        .GetAsync<Account>();
            return accounts;
        }

        public async Task<BestBids> GetBestBidAsk(string product_ids)
        {
            var bestBidAsk = await _requestBuilder.AddResource(Resources.BESTBIDASK)
                                 .AddQueryParameters(GetQueryParameters(product_ids))
                                 .GetAsync<BestBids>();
            return bestBidAsk;
        }

        public async Task<Instrument?> GetInstrumentsAsync(params string[] query)
        {
            var instruments = await _requestBuilder
                                        .AddResource(Resources.PRODUCTS)
                                        .AddQueryParameters(GetQueryParameters(query))
                                        .GetAsync<Instrument?>();

            return instruments;
        }

        public async Task<Orders?> GetOrdersAsync(params string[] query)
        {
            var orders = await _requestBuilder.AddResource(Resources.ORDERS)
                                  .AddQueryParameters(GetQueryParameters(query))
                                  .GetAsync<Orders>();

            return orders;
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
