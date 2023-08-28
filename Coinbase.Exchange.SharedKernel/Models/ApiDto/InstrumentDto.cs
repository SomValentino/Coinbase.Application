using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.ApiDto
{
    public class InstrumentDto
    {
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        [JsonProperty("product_type")]
        public string? ProductType { get; set; }
        [JsonProperty("product_ids")]
        public IEnumerable<string>? ProductIds { get; set; }
        [JsonProperty("contract_expiry_type")]
        public string? ContractExpiryType { get; set; }
    }
}
