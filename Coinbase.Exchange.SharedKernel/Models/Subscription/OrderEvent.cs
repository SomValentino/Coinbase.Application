using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.Subscription
{
    public class OrderEvent
    {
        public string Type { get; set; }
        [JsonProperty("product_id")]
        public string ProductId { get; set; }
        public List<OrderUpdate> Updates { get; set; }
    }
}
