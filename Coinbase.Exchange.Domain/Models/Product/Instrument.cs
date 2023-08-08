using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Domain.Models.Products
{
    public class Instrument
    {
        public IEnumerable<Product> Products { get; set; }
        public int Num_Products { get; set; }
    }
}
