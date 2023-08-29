using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.Subscription
{
    public class CandleEvent
    {
        public string Type { get; set; }
        public List<CandleDetails> Candles { get; set; }
    }
}
