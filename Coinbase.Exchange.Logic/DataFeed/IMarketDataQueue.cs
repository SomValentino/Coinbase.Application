using Coinbase.Exchange.SharedKernel.Models.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Logic.DataFeed
{
    public interface IMarketDataQueue
    {
        ValueTask EnqueueAsync(MarketData marketData);

        ValueTask<MarketData> DequeueAsync(CancellationToken cancellationToken);
    }
}
