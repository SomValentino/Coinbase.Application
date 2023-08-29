using Coinbase.Exchange.SharedKernel.Models.Subscription;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Logic.Processors
{
    public class TickerChannelProcessor : ChannelProcessor
    {
        public override string Channel => "ticker";

        public override IEnumerable<MarketDataResult> Process(string message)
        {
            var result = new List<MarketDataResult>();
            var tickerData = JsonConvert.DeserializeObject<Ticker>(message);
            var tickers = tickerData!.Events.SelectMany(_ => _.Tickers);


            foreach (var priceData in tickers)
            {
                result.Add(new MarketDataResult { Type = "Price", Data = priceData.Price, Instrument = priceData.ProductId });
            }

            return result;
        }
    }
}
