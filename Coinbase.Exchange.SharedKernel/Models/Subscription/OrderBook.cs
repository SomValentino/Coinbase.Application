using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.Subscription
{
    public class OrderBook : SocketDataBase
    {
        public List<OrderEvent> Events { get; set; }
    }
}
