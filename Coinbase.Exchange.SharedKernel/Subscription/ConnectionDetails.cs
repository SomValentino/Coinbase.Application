using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Subscription
{
    public class ConnectionDetails
    {
        public string type { get; set; }
        public string channel { get; set; }
        public string api_key { get; set; }
        public string signature { get; set; }
        public long timestamp { get; set; }
        public IEnumerable<string> product_ids { get; set;}

    }
}
