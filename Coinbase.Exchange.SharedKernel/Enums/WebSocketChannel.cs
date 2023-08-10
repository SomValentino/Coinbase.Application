using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Enums
{
    public enum WebSocketChannel
    {
        heartbeats = 1,
        candles,
        status,
        ticker,
        ticker_batch,
        level2,
        user,
        market_trades
    }
}
