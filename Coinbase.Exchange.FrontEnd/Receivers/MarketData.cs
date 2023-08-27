using Coinbase.Exchange.SharedKernel.Models.ApiDto;
using Coinbase.Exchange.SharedKernel.Models.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.FrontEnd.Receivers
{
    public class MarketData
    {
        public decimal Price { get; set; }
        public List<OrderBookUpdate> Bids { get; set; }
        public List<OrderBookUpdate> Offers { get; set; }
    }
}
