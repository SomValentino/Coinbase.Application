using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.ApiDto
{
    public class OrderBookUpdate
    {
        public string Instrument { get; set; }
        public string Side { get; set; }
        public decimal Price { get; set; }
        public DateTime EventTime { get; set; }
    }
}
