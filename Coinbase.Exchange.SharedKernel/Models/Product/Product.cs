using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.Products
{
    public class Product
    {
        public string Product_Id { get; set; }
        public string Price { get; set; }
        public string Product_Type { get; set; }
        public string Quote_Currency_Id { get; set; }
        public string Base_Currency_Id { get; set; }
        public string Mid_Market_Price { get; set; }
        public string Status { get; set; }
        public bool Is_Disabled { get; set; }
    }
}
