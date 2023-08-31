using Ardalis.GuardClauses;
using Coinbase.Exchange.API.Extensions;
using Coinbase.Exchange.Logic.DataFeed;
using Coinbase.Exchange.Logic.Factory;
using Coinbase.Exchange.SharedKernel.Models.ApiDto;
using Coinbase.Exchange.SharedKernel.Models.Bids;
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
        private readonly IProcessorFactory _processorFactory;
        private readonly ILogger<ExchangeWorker> _logger;

        public ExchangeWorker(IMarketDataQueue marketDataQueue, 
            IHubContext<ExchangeHub.ExchangeHub> hubContext,
            IProcessorFactory processorFactory,
            ILogger<ExchangeWorker> logger)
        {
            _marketDataQueue = Guard.Against.Null(marketDataQueue, nameof(marketDataQueue));
            _hubContext = Guard.Against.Null(hubContext,nameof(hubContext));
            _processorFactory = Guard.Against.Null(processorFactory, nameof(processorFactory));
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
                    var channelProcessor = _processorFactory.GetProcessor(marketData.Channel);

                    Guard.Against.Null(Guard.Against.Null(channelProcessor, nameof(channelProcessor)), "Channel Processor not found");

                    var results = channelProcessor.Process(marketData.Data);

                    foreach(var result in results)
                    {
                        await result.SendToClientsAsync(_hubContext);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
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
