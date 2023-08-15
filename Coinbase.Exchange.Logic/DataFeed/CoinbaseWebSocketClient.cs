using Coinbase.Exchange.Logic.Security;
using Coinbase.Exchange.SharedKernel.Enums;
using Coinbase.Exchange.SharedKernel.Models.Subscription;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Websocket.Client;

namespace Coinbase.Exchange.Logic.DataFeed
{
    public class CoinbaseWebSocketClient : ICoinbaseWebSocketClient, IDisposable
    {
        private readonly ISecretManager _secretManager;
        private readonly IMarketDataQueue _marketQueue;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CoinbaseWebSocketClient> _logger;
        private WebsocketClient _client;
        private bool IsInitialized;
        private HashSet<string> product_ids;
        private List<WebSocketChannel> channels = new List<WebSocketChannel> { WebSocketChannel.ticker, WebSocketChannel.level2 };

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

            _client.MessageReceived.Subscribe(async msg =>
            {
                await SaveMessage(msg);
               
            });
            _client.Start().Wait();
            IsInitialized = true;
        }

        private async Task SaveMessage(ResponseMessage msg)
        {
            var message = JsonConvert.DeserializeObject<SocketDataBase>(msg.Text);

            if(Enum.TryParse(message!.Channel,true, out WebSocketChannel channel))
            {
                var marketData = new MarketData
                {
                    Channel = channel,
                    SequenceNumber = message.SequenceNum,
                    Data = msg.Text
                };

                await _marketQueue.EnqueueAsync(marketData);
            }
        }

        private async Task SendSubscriptionRequests()
        {
            var api_key = "BUIkOdka61km8Slz";
           
            foreach(var channel in channels)
            {
                
                (var timestamp, var signature) = GetTimeStampSignature(channel);
                var subscription = new ConnectionDetails
                {
                    Type = ConnectionType.subscribe.ToString(),
                    ProductIds = product_ids.ToArray(),
                    Channel = channel.ToString(),
                    Signature = signature,
                    Timestamp = timestamp.ToString(),
                    ApiKey = api_key
                };


                var data = JsonConvert.SerializeObject(subscription);

                _client.Send(data);
            }
        }

        private (long timestamp, string signature) GetTimeStampSignature(WebSocketChannel channel)
        {
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            var payload = $"{timestamp}{channel}{string.Join(",", product_ids)}";
            var secretManager = new SecretManager();

            var signature = secretManager.GetSignature(payload, "QzapTGg6JBa0P533ITVOoOvAMzByu0Wp");
            return (timestamp, signature);
        }

        private bool AddProductIds(IEnumerable<string> productIds)
        {
            var reconnect = false;
            foreach (var productId in productIds)
            {
                if (!product_ids.Contains(productId))
                {
                    reconnect = true;
                    product_ids.Add(productId);
                }
            }
            return reconnect;
        }
        private void RemoveProductIds(IEnumerable<string> productIds)
        {

            foreach (var productId in productIds)
            {
                if (product_ids.Contains(productId))
                {
                    product_ids.Remove(productId);
                }
            }
        }


        public CoinbaseWebSocketClient(ISecretManager secretManager,
            IMarketDataQueue marketDataQueue,
            IConfiguration configuration,
            ILogger<CoinbaseWebSocketClient> logger)
        {
            _secretManager = secretManager;
            _marketQueue = marketDataQueue;
            _configuration = configuration;
            _logger = logger;
            product_ids = new HashSet<string>();
        }

        
        public void Dispose()
        {
            _client.Dispose();
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

        public async Task UnSubscribe(IEnumerable<string>? instruments = null)
        {
            var products = instruments ?? new HashSet<string>(product_ids);
            RemoveProductIds(products);

            var api_key = "";
            foreach (var channel in channels)
            {
                (var timestamp, var signature) = GetTimeStampSignature(channel);
                var subscription = new ConnectionDetails
                {
                    Type = ConnectionType.unsubscribe.ToString(),
                    ProductIds = products,
                    Channel = channel.ToString(),
                    Signature = signature,
                    Timestamp = timestamp.ToString(),
                    ApiKey = api_key
                };


                var data = JsonConvert.SerializeObject(subscription);

                _client.Send(data);
            }

        }

        
    }

}
