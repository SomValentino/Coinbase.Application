using Coinbase.Exchange.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.Subscription
{
    public class MarketData
    {
        public WebSocketChannel Channel { get; set; }
        public int SequenceNumber { get; set; }
        public string Data { get; set; }
    }
}
