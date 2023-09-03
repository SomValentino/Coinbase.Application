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
        public static Dictionary<string, double> LastTimeUpdateStore = new Dictionary<string, double>();

        public override IEnumerable<MarketDataResult> Process(string message)
        {
            var result = new List<MarketDataResult>();

            var candleData = JsonConvert.DeserializeObject<Candle>(message);

            var candleDetails = candleData!.Events.SelectMany(_ => _.Candles);

            var instrument = candleDetails.First().product_id;

            if(!LastTimeUpdateStore.ContainsKey(instrument))
            {
                LastTimeUpdateStore.Add(instrument, 0.0d);
            }

            double endTime = ((DateTimeOffset)(candleData.Timestamp)).ToUnixTimeSeconds();
            var range = LastTimeUpdateStore[instrument] == 0.0d ? 150 : endTime - LastTimeUpdateStore[instrument];
            LastTimeUpdateStore[instrument] = endTime;
            var interval = range/candleDetails.Count();

            double startTime = endTime - 300;

            double i = 0;


            candleDetails = candleDetails.Select(_ =>
            {
                _.Date = candleData.Timestamp;
                _.Time = startTime + i;
                startTime = _.Time;
                i = interval;
                return _;
            });
                    

            if(candleDetails.Any() )
            {
                var serialized_candles = JsonConvert.SerializeObject(candleDetails);
                result.Add(new MarketDataResult { Data = serialized_candles, Instrument = instrument, Type = MessageType.Candles });
            }

            return result;
        }
    }
}
