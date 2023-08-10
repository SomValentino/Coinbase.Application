using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.Bids
{
    public class PriceBooks
    {
        public string Product_id { get; set; }
        public List<Bid> Bids { get; set; }
        public List<Ask> Asks { get; set; }
        public string Time { get; set; }
    }
}
