using Coinbase.Exchange.API.Extensions;
using Coinbase.Exchange.Logic.DataFeed;
using Coinbase.Exchange.SharedKernel.Enums;
using Coinbase.Exchange.SharedKernel.Models.ApiDto;
using Coinbase.Exchange.SharedKernel.Models.Bids;
using Coinbase.Exchange.SharedKernel.Models.Subscription;
using Microsoft.AspNetCore.Mvc;
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

                            var bids = orderUpdates.Where(_ => _.Side == "bid")
                                .Select(_ => (_,decimal.Parse(_.PriceLevel, CultureInfo.InvariantCulture)));

                            var offers = orderUpdates.Where(_ => _.Side == "offer")
                                .Select(_ => (_, decimal.Parse(_.PriceLevel, CultureInfo.InvariantCulture)));

                            var bidqueue = new PriorityQueue<OrderUpdate, decimal>(bids,new OrderUpdateComparer());
                            if (bidqueue.Count > 0)
                            {
                                var bestBid = bidqueue.Peek();

                                var priceUpdate = new PriceUpdate
                                {
                                    Instrument = product_Id,
                                    Price = decimal.Parse(bestBid.PriceLevel, CultureInfo.InvariantCulture)
                                };

                                await priceUpdate.SendToClientsAsync(nameof(bestBid),_hubContext); 
                            }

                            var offerqueue = new PriorityQueue<OrderUpdate,decimal>(offers);

                            if(offerqueue.Count > 0)
                            {
                                var bestOffer = offerqueue.Peek();
                                var priceUpdate = new PriceUpdate
                                {
                                    Instrument = product_Id,
                                    Price = decimal.Parse(bestOffer.PriceLevel, CultureInfo.InvariantCulture)
                                };

                                await priceUpdate.SendToClientsAsync(nameof(bestOffer), _hubContext);
                            }

                            var orderBookUpdate = orderUpdates.Select(_ => new OrderBookUpdate
                            {
                                Instrument = product_Id,
                                Price = decimal.Parse(_.PriceLevel, CultureInfo.InvariantCulture),
                                Side = _.Side,
                                EventTime = _.EventTime
                            });

                            

                            await orderBookUpdate.SendToClientsAsync(product_Id, _hubContext);
                            
                            break;
                        case "ticker":
                            var tickerData = JsonConvert.DeserializeObject<Ticker>(marketData.Data);
                            var tickers = tickerData.Events.SelectMany(_ => _.Tickers);
                            var priceUpdates = tickers.Select(_ => new PriceUpdate
                            {
                                Instrument = _.ProductId,
                                Price = decimal.Parse(_.Price, CultureInfo.InvariantCulture)
                            });

                            foreach(var priceData in priceUpdates)
                            {
                                await priceData.SendToClientsAsync("price",_hubContext);
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
