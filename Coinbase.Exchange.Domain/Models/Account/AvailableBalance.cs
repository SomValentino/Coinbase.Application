using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Domain.Models.Account
{
    public class AvailableBalance
    {
        public string Value { get; set; }
        public string Currency { get; set; }
    }
}
