using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Subscription
{
    public class ConnectionDetails
    {
        public string Type { get; set; }
        public string Channel { get; set; }
        public string Api_key { get; set; }
        public string Signature { get; set; }
        public long Timestamp { get; set; }
        public IEnumerable<string> Product_ids { get; set;}

    }
}
