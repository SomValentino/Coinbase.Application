using Coinbase.Exchange.Domain.Specifications;
using Coinbase.Exchange.Infrastructure.Repository;
using Coinbase.Exchange.Logic.Builders;
using Coinbase.Exchange.SharedKernel.Constants;
using Coinbase.Exchange.SharedKernel.Models.Account;
using Coinbase.Exchange.SharedKernel.Models.Bids;
using Coinbase.Exchange.SharedKernel.Models.Order;
using Coinbase.Exchange.SharedKernel.Models.Products;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace Coinbase.Exchange.Logic.Services
{
    public class CoinbaseService : ICoinbaseService
    {
        private readonly IRequestBuilder _requestBuilder;
        private readonly IRepository<Domain.Entities.Instrument> _instrumentRepository;
        private readonly IRepository<Domain.Entities.Client> _clientRepository;
        private readonly ILogger<CoinbaseService> _logger;

        public CoinbaseService(IRequestBuilder requestBuilder,
            IRepository<Domain.Entities.Instrument> instrumentrepository,
            IRepository<Domain.Entities.Client> clientrepository,
            ILogger<CoinbaseService> logger)
        {
            _requestBuilder = requestBuilder;
            _instrumentRepository = instrumentrepository;
            _clientRepository = clientrepository;
            _logger = logger;
        }

        public async Task AddInstrumentSubscription(IEnumerable<string> instruments, Domain.Entities.Client client)
        {
            var instrumentsList = await _instrumentRepository.GetListAsync(new InstrumentsByNamesSpec(instruments));

            foreach(var instrument in instruments)
            {
                var instrumentInstance = instrumentsList.FirstOrDefault(_ => _.Name == instrument);

                if(instrumentInstance == null)
                {
                    var newInstrument = await _instrumentRepository.AddAsync(new Domain.Entities.Instrument()
                                        {
                                            Name = instrument
                                        });
                    newInstrument.Clients.Add(client);
                }
                else
                {
                    instrumentInstance.Clients.Add(client);
                }
            }
            await _instrumentRepository.SaveChangesAsync();
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

        public async Task<IEnumerable<string>> GetClientSubscribedInstrumentsAsync(string clientId)
        {
            var client = await _clientRepository.GetBySpecAsync(new InstrumentsByClientIdSpec(clientId));

            return client.ProductGroups.Select(x => x.Name);
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

        public async Task RemoveInstrumentSubscription(IEnumerable<string> instruments, Domain.Entities.Client client)
        {
            var instrumentsList = await _instrumentRepository.GetListAsync(new InstrumentsByNamesSpec(instruments));

            foreach (var instrument in instrumentsList) 
            {
                instrument.Clients.Remove(client);
            }

            await _instrumentRepository.SaveChangesAsync();
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
