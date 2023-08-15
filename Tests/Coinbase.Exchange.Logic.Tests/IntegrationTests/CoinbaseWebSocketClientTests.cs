using Coinbase.Exchange.Domain.Entities;
using Coinbase.Exchange.Logic.DataFeed;
using Coinbase.Exchange.Logic.Security;
using Coinbase.Exchange.SharedKernel.Enums;
using Microsoft.Extensions.Configuration;
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
        private MarketDataQueue _marketDataQueue;
        private ILogger<CoinbaseWebSocketClient> _logger;
        private readonly IConfiguration _configuration;
        private CoinbaseWebSocketClient _webSocketClient;

        public CoinbaseWebSocketClientTests()
        {

            _secretManager = new SecretManager();
            _marketDataQueue = new MarketDataQueue();

            _logger = Mock.Of<ILogger<CoinbaseWebSocketClient>>();
            _configuration = Mock.Of<IConfiguration>();
            _webSocketClient = new CoinbaseWebSocketClient(_secretManager,_marketDataQueue,_configuration, _logger);
        }


        [Fact]
        public async Task CoinbaseWebSocketClient_WhenSubscribed_ReceiveMessages()
        {
            ManualResetEvent ExitEvent = new ManualResetEvent(false);
            var uri = new Uri("wss://ws-feed.pro.coinbase.com");
            var product_ids = new[] {"ETH-USD","BTC-USD" };

            await _webSocketClient.SubScribe( product_ids);
            ExitEvent.WaitOne();
        }
    }
}
