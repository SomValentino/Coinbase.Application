using Coinbase.Exchange.SharedKernel.Constants;
using Coinbase.Exchange.SharedKernel.Models.Subscription;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Logic.Processors
{
    public class CandlesChannelProcessor : ChannelProcessor
    {
        public override string Channel => Channels.Candles;

        public override IEnumerable<MarketDataResult> Process(string message)
        {
            var result = new List<MarketDataResult>();

            var candleData = JsonConvert.DeserializeObject<Candle>(message);

            var candleDetails = candleData!.Events.SelectMany(_ => _.Candles);

            candleDetails = candleDetails.Select(_ =>
                    {
                        _.Date = candleData!.Timestamp.Date;
                        return _;
                    });

            if(candleDetails.Any() )
            {
                var serialized_candles = JsonConvert.SerializeObject(candleDetails);

                var instrument = candleDetails.First().product_id;

                result.Add(new MarketDataResult { Data = serialized_candles, Instrument = instrument, Type = MessageType.Candles });
            }

            return result;
        }
    }
}
