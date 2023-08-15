using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.Subscription
{
    public class SocketDataBase
    {
        public string Channel { get; set; }
        [JsonProperty("client_id")]
        public string ClientId { get; set; }
        public string Timestamp { get; set; }
        [JsonProperty("sequence_num")]
        public int SequenceNum { get; set; }
    }
}
