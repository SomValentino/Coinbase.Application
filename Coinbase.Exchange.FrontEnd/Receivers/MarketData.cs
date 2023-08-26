using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.FrontEnd.Receivers
{
    public class MarketData
    {
        public string Instrument { get; set; }
        public string Price { get; set; }
        public string BestBid { get; set; }
        public string BestOffer { get; set; }
        public string Bids { get; set; }
        public string Offers { get; set; }
    }
}
