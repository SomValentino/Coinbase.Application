using Coinbase.Exchange.SharedKernel.Constants;
using Coinbase.Exchange.SharedKernel.Enums;
using Coinbase.Exchange.SharedKernel.Models.Subscription;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Logic.DataFeed
{
    public class MarketDataQueue : IMarketDataQueue
    {
        private Dictionary<string, int> _dataTracker;
        private readonly Channel<MarketData> _queue;

        private void ResetDataTracker()
        {
            _dataTracker = new Dictionary<string, int>();
            foreach(var channel in Channels.OutputChannels)
            {
                _dataTracker.Add(channel, -1);
            }
        }
        public MarketDataQueue()
        {
            _queue = Channel.CreateUnbounded<MarketData>(
                    new UnboundedChannelOptions
                    {
                        SingleWriter = false,
                        SingleReader = false,
                        AllowSynchronousContinuations = true
                    });
            ResetDataTracker();
        }

       
        public async ValueTask<MarketData> DequeueAsync(CancellationToken cancellationToken)
        {
            var item = await _queue.Reader.ReadAsync(cancellationToken);

            return item;
        }

        public async ValueTask EnqueueAsync(MarketData marketData)
        {
            if(_dataTracker.ContainsKey(marketData.Channel))
            {
                var currentSequenceNumber = _dataTracker[marketData.Channel];

                if(marketData.SequenceNumber > currentSequenceNumber)
                {
                    await _queue.Writer.WriteAsync(marketData);

                    _dataTracker[marketData.Channel] = marketData.SequenceNumber;
                }
            }
        }
    }
}
