using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.SharedKernel.Models.Configuration
{
    public class ApiConfiguration
    {
        public string Apikey { get; set; }
        public string ApiSecret { get; set; }
        public string ApiBaseUrl { get; set; }
        public string WebSocketurl { get; set;}
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
        public string Role { get; set; }
    }
}
