using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.Products
{
    public class Product
    {
        [JsonProperty("product_id")]
        public string Product_Id { get; set; }
        public string Price { get; set; }
        [JsonProperty("product_type")]
        public string Product_Type { get; set; }
        [JsonProperty("quote_currency_id")]
        public string Quote_Currency_Id { get; set; }
        [JsonProperty("base_currency_id")]
        public string Base_Currency_Id { get; set; }
        [JsonProperty("mid_market_price")]
        public string Mid_Market_Price { get; set; }
        public string Status { get; set; }
        [JsonProperty("is_disabled")]
        public bool Is_Disabled { get; set; }
    }
}
