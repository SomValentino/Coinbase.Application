using Coinbase.Exchange.API.Extensions;
using Coinbase.Exchange.Logic.DataFeed;
using Coinbase.Exchange.SharedKernel.Enums;
using Coinbase.Exchange.SharedKernel.Models.Subscription;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Coinbase.Exchange.API.BackgroundServices
{
    public class ExchangeWorker : BackgroundService
    {
        private readonly IMarketDataQueue _marketDataQueue;
        private readonly IHubContext<ExchangeHub.ExchangeHub> _hubContext;
        private readonly ILogger<ExchangeWorker> _logger;

        public ExchangeWorker(IMarketDataQueue marketDataQueue, 
            IHubContext<ExchangeHub.ExchangeHub> hubContext,
            ILogger<ExchangeWorker> logger)
        {
            _marketDataQueue = marketDataQueue;
            _hubContext = hubContext;
            _logger = logger;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var marketData = await _marketDataQueue.DequeueAsync(stoppingToken);

                try
                {
                    switch (marketData.Channel)
                    {
                        case WebSocketChannel.level2:
                            var orderBook = JsonConvert.DeserializeObject<OrderBook>(marketData.Data);
                            await orderBook!.Events.SendToClientsAsync(_hubContext);
                            break;
                        case WebSocketChannel.ticker:
                            var tickerData = JsonConvert.DeserializeObject<Ticker>(marketData.Data);
                            await tickerData!.Events.SelectMany(_ => _.Tickers).SendToClientsAsync(_hubContext);
                            break;
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
