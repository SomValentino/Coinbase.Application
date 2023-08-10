using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.Account
{
    public class AccountEntry
    {
        public string Uuid { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public AvailableBalance Available_Balance { get; set; }
        public bool Default { get; set; }
        public bool Active { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public object Deleted_at { get; set; }
        public string Type { get; set; }
        public bool Ready { get; set; }
        public Hold hold { get; set; }
    }
}
