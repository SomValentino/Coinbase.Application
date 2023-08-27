using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.Products
{
    public class Instrument
    {
        public IEnumerable<Product> Products { get; set; }
        [JsonProperty("num_products")]
        public int NumProducts { get; set; }
    }
}
