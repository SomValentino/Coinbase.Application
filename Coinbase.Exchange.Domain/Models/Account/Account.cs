using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Domain.Models.Account
{
    public class Account
    {
        public List<AccountEntry> Accounts { get; set; }
        public bool Has_Next { get; set; }
        public string Cursor { get; set; }
        public int Size { get; set; }
    }
}
