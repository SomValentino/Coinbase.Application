using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Domain.Models.Order
{
    public class StopLimitGtd
    {
        public double Base_size { get; set; }
        public string Limit_price { get; set; }
        public string Stop_price { get; set; }
        public DateTime End_time { get; set; }
        public string Stop_direction { get; set; }
    }
}
