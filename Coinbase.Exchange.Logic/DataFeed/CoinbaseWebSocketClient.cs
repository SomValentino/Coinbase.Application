using Coinbase.Exchange.Logic.Security;
using Coinbase.Exchange.SharedKernel.Enums;
using Coinbase.Exchange.SharedKernel.Subscription;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Logic.DataFeed
{
    public class CoinbaseWebSocketClient
    {
        private readonly ISecretManager _secretManager;
        private readonly ILogger<CoinbaseWebSocketClient> _logger;
        private ClientWebSocket _clientWebSocket;
        private CancellationTokenSource _socketLoopTokenSource;

        public CoinbaseWebSocketClient(ISecretManager secretManager,ILogger<CoinbaseWebSocketClient> logger)
        {
            _secretManager = secretManager;
            _logger = logger;
            _clientWebSocket = new ClientWebSocket();
        }

        public async void Subscribe(Uri uri, IEnumerable<string> productIds, 
            WebSocketChannel channel = WebSocketChannel.level2)
        {
            _socketLoopTokenSource = new CancellationTokenSource();
            
            try
            {
                await _clientWebSocket.ConnectAsync(uri, CancellationToken.None);
                _ = Task.Run(() => SocketProcessingLoopAsync(productIds,channel).ConfigureAwait(false));

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task SocketProcessingLoopAsync(IEnumerable<string> productIds, WebSocketChannel channel)
        {
            var cancellationToken = _socketLoopTokenSource.Token;
            try
            {
                var api_key_setting = await _secretManager.GetSetting("api_key");
                (string signature, long timestamp) = await GetTimeStampSignature(productIds, channel);

                while (_clientWebSocket.State != WebSocketState.Closed && !cancellationToken.IsCancellationRequested)
                {

                    var connectionDetails = new ConnectionDetails
                    {
                        Type = ConnectionType.subcribe.ToString(),
                        Channel = channel.ToString(),
                        Api_key = api_key_setting.Value!,
                        Product_ids = productIds,
                        Signature = signature,
                        Timestamp = timestamp
                    };

                    var serializedData = JsonConvert.SerializeObject(connectionDetails);

                    await _clientWebSocket.SendAsync(Encoding.ASCII.GetBytes(serializedData), WebSocketMessageType.Text, true, cancellationToken);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<(string signature,long timestamp)> GetTimeStampSignature(IEnumerable<string> productIds, WebSocketChannel channel)
        {
            var api_secret_settiing = await _secretManager.GetSetting("api_secret");
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

            var payload = $"{timestamp}{channel}{string.Join(",", productIds)}";

            var signature = _secretManager.GetSignature(payload, api_secret_settiing.Value!);

            return (signature, timestamp);
        }
    }
}
