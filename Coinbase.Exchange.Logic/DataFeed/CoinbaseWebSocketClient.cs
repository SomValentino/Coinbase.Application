using Coinbase.Exchange.Logic.Security;
using Coinbase.Exchange.SharedKernel.Constants;
using Coinbase.Exchange.SharedKernel.Enums;
using Coinbase.Exchange.SharedKernel.Models.Configuration;
using Coinbase.Exchange.SharedKernel.Models.Subscription;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Websocket.Client;

namespace Coinbase.Exchange.Logic.DataFeed
{
    public class CoinbaseWebSocketClient : ICoinbaseWebSocketClient, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMarketDataQueue _marketQueue;
        private readonly ApiConfiguration _apiConfig;
        private readonly ILogger<CoinbaseWebSocketClient> _logger;
        private WebsocketClient _client;
        private bool IsInitialized;
        private HashSet<string> product_ids;

        private async Task Initialize()
        {
            _client = new WebsocketClient(new Uri(_apiConfig.WebSocketurl));

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

            var marketData = new MarketData
            {
                Channel = message!.Channel,
                SequenceNumber = message.SequenceNum,
                Data = msg.Text
            };

            await _marketQueue.EnqueueAsync(marketData);
        }

        private async Task SendSubscriptionRequests()
        {
            var api_key = _apiConfig.Apikey;
           
            foreach(var channel in Channels.InputChannels)
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

        private (long timestamp, string signature) GetTimeStampSignature(string channel)
        {
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            var payload = $"{timestamp}{channel}{string.Join(",", product_ids)}";
            var secretManager = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ISecretManager>();

            var signature = secretManager.GetSignature(payload, _apiConfig.ApiSecret);
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


        public CoinbaseWebSocketClient(IServiceProvider serviceProvider,
            IMarketDataQueue marketDataQueue,
            IOptions<ApiConfiguration> apiConfig,
            ILogger<CoinbaseWebSocketClient> logger)
        {
            _serviceProvider = serviceProvider;
            _marketQueue = marketDataQueue;
            _apiConfig = apiConfig.Value;
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

            var api_key = _apiConfig.Apikey;
            foreach (var channel in Channels.InputChannels)
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
