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
        private CoinbaseWebsocketClient _client;
        private bool IsInitialized;
        private HashSet<string> product_ids;
        ManualResetEvent _exitEvent = new ManualResetEvent(false);

        private async Task Initialize()
        {
            var communicator = new CoinbaseWebsocketCommunicator(new Uri("wss://advanced-trade-ws.coinbase.com"));
            communicator.Name = "coinbase-1";
            communicator.ReconnectTimeout = TimeSpan.FromMinutes(1);

            _client = new CoinbaseWebsocketClient(communicator);

            SubscribeToStreams();

            communicator.ReconnectionHappened.Subscribe(async type =>
            {
                _logger.LogInformation("Reconnection happend, type: {type}", type);

                await SendSubscriptionRequests();
            });

            communicator.Start().Wait();
            IsInitialized = true;
        }

        private async Task SendSubscriptionRequests()
        {
            var api_key = "";
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            var payload = $"{timestamp}heartbeats{string.Join(",", product_ids)}";
            var secretManager = new SecretManager();

            var signature = secretManager.GetSignature(payload, "");

            var subscription = new ConnectionDetails
            {
                ProductIds = product_ids.ToArray(),
                Channel = "heartbeats",
                Signature = signature,
                Timestamp = timestamp.ToString(),
                ApiKey = api_key
            };

            _client.Send(subscription);
        }

        private void SubscribeToStreams()
        {
            _client.Streams.ErrorStream.Subscribe(x =>
            {
                _logger.LogError("$ Error received, message: {message}", x.Message);
            });

            _client.Streams.SubscribeStream.Subscribe(x =>
            {
                _logger.LogInformation($"Subscribed, " +
                                $"channels: {JsonConvert.SerializeObject(x.Channels, CoinbaseJsonSerializer.Settings)}");
            });

            _client.Streams.HeartbeatStream.Subscribe(x =>
                _logger.LogInformation($"Heartbeat received, product: {x.ProductId}, seq: {x.Sequence}, time: {x.Time}"));


            _client.Streams.TickerStream.Subscribe(x =>
                    _logger.LogInformation($"Ticker, seq: {x.Sequence} {x.ProductId}. Bid: {x.BestBid} Ask: {x.BestAsk} Last size: {x.LastSize}, Price: {x.Price}")
                );

            _client.Streams.TradesStream.Subscribe(x =>
            {
                _logger.LogInformation($"Trade executed, seq: {x.Sequence} [{x.ProductId}] {x.TradeSide} price: {x.Price} size: {x.Size}");
            });

            _client.Streams.OrderBookSnapshotStream.Subscribe(x =>
            {
                _logger.LogInformation($"OB snapshot [{x.ProductId}] bids: {x.Bids.Length}, asks: {x.Asks.Length}");
            });

            _client.Streams.OrderBookUpdateStream.Subscribe(x =>
            {
                _logger.LogInformation($"OB updates [{x.ProductId}] changes: {x.Changes.Length}");
            });
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
