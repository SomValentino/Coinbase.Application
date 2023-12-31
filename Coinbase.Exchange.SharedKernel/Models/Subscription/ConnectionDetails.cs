﻿using Coinbase.Client.Websocket.Requests;
using Coinbase.Exchange.SharedKernel.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.Subscription
{
    public class ConnectionDetails
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("channel")]
        public string Channel { get; set; }
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }
        [JsonProperty("signature")]
        public string Signature { get; set; }
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
        [JsonProperty("product_ids")]
        public IEnumerable<string> ProductIds { get; set; }

    }
}
