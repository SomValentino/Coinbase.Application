using Coinbase.Client.Websocket.Channels;
using Coinbase.Client.Websocket.Client;
using Coinbase.Client.Websocket.Communicator;
using Coinbase.Client.Websocket.Json;
using Coinbase.Client.Websocket.Requests;
using Coinbase.Exchange.Logic.Security;
using Coinbase.Exchange.SharedKernel.Enums;
using Coinbase.Exchange.SharedKernel.Subscription;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Websocket.Client;

namespace Coinbase.Exchange.Logic.DataFeed
{
    public class CoinbaseWebSocketClient : ICoinbaseWebSocketClient, IDisposable
    {
        private readonly ISecretManager _secretManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CoinbaseWebSocketClient> _logger;
        private WebsocketClient _client;
        private bool IsInitialized;
        private HashSet<string> product_ids;
        ManualResetEvent _exitEvent = new ManualResetEvent(false);

        private async Task Initialize()
        {
            _client = new WebsocketClient(new Uri("wss://advanced-trade-ws.coinbase.com"));

            _client.Name = "Coinbase-1";
            _client.ReconnectTimeout = TimeSpan.FromSeconds(120);
            _client.ErrorReconnectTimeout = TimeSpan.FromSeconds(30);
            _client.ReconnectionHappened.Subscribe(async info =>
            {
                await SendSubscriptionRequests();
            });

            _client.MessageReceived.Subscribe(msg =>
            {
                Console.WriteLine($"Message received: {msg}");
               
            });
            _client.Start().Wait();
            IsInitialized = true;
        }

        private async Task SendSubscriptionRequests()
        {
            var api_key = "BUIkOdka61km8Slz";
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            var payload = $"{timestamp}ticker{string.Join(",", product_ids)}";
            var secretManager = new SecretManager();

            var signature = secretManager.GetSignature(payload, "QzapTGg6JBa0P533ITVOoOvAMzByu0Wp");



            var subscription = new ConnectionDetails
            {
                ProductIds = product_ids.ToArray(),
                Channel = "ticker",
                Signature = signature,
                Timestamp = timestamp.ToString(),
                ApiKey = api_key
            };

            var data = JsonConvert.SerializeObject(subscription);

            _client.Send(data);
        }

       

        public CoinbaseWebSocketClient(ISecretManager secretManager,
            IConfiguration configuration,
            ILogger<CoinbaseWebSocketClient> logger)
        {
            _secretManager = secretManager;
            _configuration = configuration;
            _logger = logger;
            product_ids = new HashSet<string>();
        }

        private bool AddProductIds(IEnumerable<string> productIds)
        {
            var reconnect = false;
            foreach (var productId in productIds)
            {
                if(!product_ids.Contains(productId))
                {
                    reconnect = true;
                    product_ids.Add(productId);
                }
            }
            return reconnect;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task SubScribe(IEnumerable<string> instruments)
        {
            var reconnect = AddProductIds(instruments);
            if (!IsInitialized)
            {
                await Initialize();
                return;
            }
            if(reconnect)
            {
                await SendSubscriptionRequests();
            }
        }

        public Task UnSubscribe(IEnumerable<string> instruments)
        {
            throw new NotImplementedException();
        }

        
    }

}
