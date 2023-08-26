using Coinbase.Exchange.API.Extensions;
using Coinbase.Exchange.Logic.DataFeed;
using Coinbase.Exchange.SharedKernel.Models.ApiDto;
using Coinbase.Exchange.SharedKernel.Models.Subscription;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Globalization;

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
                    switch (marketData.Channel.ToString())
                    {
                        case "l2_data":
                            var orderBook = JsonConvert.DeserializeObject<OrderBook>(marketData.Data);

                            var product_Id = orderBook!.Events.FirstOrDefault()!.ProductId;

                            var orderUpdates = orderBook!.Events
                                .SelectMany(_ => _.Updates);

                            var bids = orderUpdates.Where(_ => _.Side == "bid");

                            var offers = orderUpdates.Where(_ => _.Side == "offer");

                            if (bids.Any())
                            {
                                await bids.First().PriceLevel.SendToClientsAsync(product_Id, "BestBid", _hubContext);
                                await string.Join("\n",bids.Select(_ => $"{_.PriceLevel} | {_.Quantity}"))
                                    .SendToClientsAsync(product_Id, "Bids", _hubContext);
                            }

                            if (offers.Any())
                            {
                                await offers.First().PriceLevel.SendToClientsAsync(product_Id, "BestOffer", _hubContext);
                                await string.Join("\n", offers.Select(_ => $"{_.PriceLevel} | {_.Quantity}"))
                                    .SendToClientsAsync(product_Id, "Offers", _hubContext);
                            }
                            break;
                        case "ticker":
                            var tickerData = JsonConvert.DeserializeObject<Ticker>(marketData.Data);
                            var tickers = tickerData.Events.SelectMany(_ => _.Tickers);
                            

                            foreach(var priceData in tickers)
                            {
                                await priceData.Price.SendToClientsAsync(priceData.ProductId,"Price",_hubContext);
                            }
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
