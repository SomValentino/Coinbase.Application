using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Domain.Models.Order
{
    public class StopLimitGtc
    {
        public string Base_size { get; set; }
        public string Limit_price { get; set; }
        public string Stop_price { get; set; }
        public string Stop_direction { get; set; }
    }
}
