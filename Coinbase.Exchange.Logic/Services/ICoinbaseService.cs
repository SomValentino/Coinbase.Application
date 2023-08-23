using Coinbase.Exchange.SharedKernel.Models.Account;
using Coinbase.Exchange.SharedKernel.Models.Bids;
using Coinbase.Exchange.SharedKernel.Models.Order;
using Coinbase.Exchange.SharedKernel.Models.Products;

namespace Coinbase.Exchange.Logic.Services
{
    public interface ICoinbaseService
    {
        Task<Instrument?> GetInstrumentsAsync(params string[] query);
        Task<Account?> GetAllAccountAsync(params string[] query);
        Task<AccountEntry?> GetAccountEntryAsync(string uuid);

        Task<Orders?> GetOrdersAsync(params string[] query );
        Task<BestBids?> GetBestBidAsk(string product_ids);
        Task AddInstrumentSubscription(IEnumerable<string> instrument, Domain.Entities.Client client);
        Task RemoveInstrumentSubscription(IEnumerable<string> instrument, Domain.Entities.Client client);
    }
}
