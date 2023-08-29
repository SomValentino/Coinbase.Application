using Coinbase.Exchange.SharedKernel.Models.ApiDto;
using Coinbase.Exchange.SharedKernel.Models.Subscription;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Logic.Processors
{
    public class L2ChannelProcessor : ChannelProcessor
    {
        public override string Channel => "l2_data";

        public override IEnumerable<MarketDataResult> Process(string message)
        {
            var result = new List<MarketDataResult>();
            var orderBook = JsonConvert.DeserializeObject<OrderBook>(message);

            var product_Id = orderBook!.Events.FirstOrDefault()!.ProductId;

            var orderUpdates = orderBook!.Events
                .SelectMany(_ => _.Updates);

            var bids = orderUpdates.Where(_ => _.Side == "bid");

            var offers = orderUpdates.Where(_ => _.Side == "offer");

            if (bids.Any())
            {

                var serialized_bids = JsonConvert.SerializeObject(bids.Select(_ => new OrderBookUpdate
                {
                    Price = decimal.Parse(_.PriceLevel, CultureInfo.InvariantCulture),
                    Quantity = Math.Round(decimal.Parse(_.Quantity, CultureInfo.InvariantCulture), 4),
                    EventTime = _.EventTime
                }));

                result.Add(new MarketDataResult { Type = "Bids", Data = serialized_bids, Instrument = product_Id });
            }

            if (offers.Any())
            {
                var serialized_offers = JsonConvert.SerializeObject(offers.Select(_ => new OrderBookUpdate
                {
                    Price = decimal.Parse(_.PriceLevel, CultureInfo.InvariantCulture),
                    Quantity = Math.Round(decimal.Parse(_.Quantity, CultureInfo.InvariantCulture), 4),
                    EventTime = _.EventTime
                }));

                result.Add(new MarketDataResult { Type = "Offers", Data = serialized_offers, Instrument = product_Id });
            }

            return result;
        }
    }
}
