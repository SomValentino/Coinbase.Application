using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.FrontEnd.Receivers
{
    public class MarketDataEventArgs : EventArgs
    {
        public Dictionary<string,MarketData> Store { get; set; }
        public string Type { get; set; }
    }
}
