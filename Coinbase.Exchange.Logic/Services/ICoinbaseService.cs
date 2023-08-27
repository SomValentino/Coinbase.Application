using Coinbase.Exchange.SharedKernel.Models.Account;
using Coinbase.Exchange.SharedKernel.Models.Bids;
using Coinbase.Exchange.SharedKernel.Models.Order;
using Coinbase.Exchange.SharedKernel.Models.Products;

namespace Coinbase.Exchange.Logic.Services
{
    public interface ICoinbaseService
    {
        Task<Instrument?> GetInstrumentsAsync(Dictionary<string,string> queries);
        Task<Account?> GetAllAccountAsync(Dictionary<string, string> queries);
        Task<AccountEntry?> GetAccountEntryAsync(string uuid);

        Task<Orders?> GetOrdersAsync(Dictionary<string, string> queries );
        Task<BestBids?> GetBestBidAsk(string product_ids);
        Task<IEnumerable<string>> GetClientSubscribedInstrumentsAsync(string clientId);
        Task AddInstrumentSubscription(IEnumerable<string> instrument, Domain.Entities.Client client);
        Task RemoveInstrumentSubscription(IEnumerable<string> instrument, Domain.Entities.Client client);
    }
}
