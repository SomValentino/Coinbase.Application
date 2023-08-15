using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.Subscription
{
    public class OrderUpdate
    {
        public string Side { get; set; }
        [JsonProperty("event_time")]
        public DateTime EventTime { get; set; }
        [JsonProperty("price_level")]
        public string PriceLevel { get; set; }
        [JsonProperty("new_quantity")]
        public string Quantity { get; set; }
    }
}
