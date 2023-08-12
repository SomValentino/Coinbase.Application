using Coinbase.Exchange.Logic.Security;
using Coinbase.Exchange.SharedKernel.Enums;
using Coinbase.Exchange.SharedKernel.Subscription;
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
    public class CoinbaseWebSocketClient : ICoinbaseWebSocketClient,IDisposable
    {
        private readonly ISecretManager _secretManager;
        private readonly ILogger<CoinbaseWebSocketClient> _logger;
        private WebsocketClient _client;
        private bool IsInitialized;
        private List<string> _container = new List<string>();

        private async Task Initialize(Uri uri)
        {
            _client = new WebsocketClient(uri);

            _client.ReconnectTimeout = TimeSpan.FromSeconds(30);
            _client.ReconnectionHappened.Subscribe(info => _logger.LogInformation("{info}",info));
              
            _client.DisconnectionHappened.Subscribe(info => _logger.LogInformation("{info}", info));

            _client.MessageReceived.Subscribe(msg => {
                StartMessageProcessingAsync(msg);
             });
            await _client.Start();

            IsInitialized = true;
        }

        private void StartMessageProcessingAsync(ResponseMessage message)
        {
            _container.Add(message.Text);
        }

        private async Task SendSubcriptionMessage(IEnumerable<string> instruments, WebSocketChannel channel, ConnectionType connectionType = ConnectionType.subcribe)
        {
            var api_key_setting = await _secretManager.GetSetting("BUIkOdka61km8Slz");
            (string signature, long timestamp) = await GetTimeStampSignature(instruments, channel);

            var connectionDetails = new ConnectionDetails
            {
                type = connectionType.ToString(),
                channel = channel.ToString(),
                api_key = api_key_setting.Value!,
                product_ids = instruments,
                signature = signature,
                timestamp = timestamp
            };

            var serializedData = JsonConvert.SerializeObject(connectionDetails);

            _ = Task.Run(() => _client.Send(serializedData));
        }

        private async Task<(string signature, long timestamp)> GetTimeStampSignature(IEnumerable<string> productIds, WebSocketChannel channel)
        {
            var api_secret_settiing = await _secretManager.GetSetting("QzapTGg6JBa0P533ITVOoOvAMzByu0Wp");
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

            var payload = $"{timestamp}{channel}{string.Join(",", productIds)}";

            var signature = _secretManager.GetSignature(payload, api_secret_settiing.Value!);

            return (signature, timestamp);
        }

        public CoinbaseWebSocketClient(ISecretManager secretManager, ILogger<CoinbaseWebSocketClient> logger)
        {
            _secretManager = secretManager;
            _logger = logger;
        }
        public async Task SubScribe(Uri uri, IEnumerable<string> instruments, WebSocketChannel channel)
        {
            if (!IsInitialized)
            {
                await Initialize(uri);
            }

            await SendSubcriptionMessage(instruments, channel);
        }

        public async Task UnSubscribe(Uri uri,IEnumerable<string> instruments, WebSocketChannel channel)
        {
            await SendSubcriptionMessage(instruments, channel, ConnectionType.unsubscribe);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
