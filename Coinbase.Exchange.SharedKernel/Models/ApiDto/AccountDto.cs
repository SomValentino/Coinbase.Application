using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.ApiDto
{
    public class AccountDto
    {
        public int? Limit { get; set; }
        public string? Cursor { get; set; }
    }
}
