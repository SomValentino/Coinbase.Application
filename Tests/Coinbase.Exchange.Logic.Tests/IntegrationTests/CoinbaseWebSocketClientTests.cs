using Coinbase.Exchange.Domain.Entities;
using Coinbase.Exchange.Logic.DataFeed;
using Coinbase.Exchange.Logic.Security;
using Coinbase.Exchange.SharedKernel.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Logic.Tests.IntegrationTests
{
    public class CoinbaseWebSocketClientTests
    {
        private SecretManager _secretManager;
        private ILogger<CoinbaseWebSocketClient> _logger;
        private CoinbaseWebSocketClient _webSocketClient;

        public CoinbaseWebSocketClientTests()
        {

            _secretManager = new SecretManager();

            _logger = Mock.Of<ILogger<CoinbaseWebSocketClient>>();
            _webSocketClient = new CoinbaseWebSocketClient(_secretManager, _logger);
        }


        [Fact]
        public async Task CoinbaseWebSocketClient_WhenSubscribed_ReceiveMessages()
        {
            var uri = new Uri("wss://advanced-trade-ws.coinbase.com");
            var product_ids = new[] {"ETH-USD", "ETH-EUR" };
            var channel = WebSocketChannel.level2;

            await _webSocketClient.SubScribe(uri, product_ids, channel);
            await Task.Delay(3000);
        }
    }
}
