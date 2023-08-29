using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.Subscription
{
    public class MarketDataResult
    {
        public string Type { get; set; }
        public string Data { get; set; }
        public string Instrument { get; set; }
    }
}
