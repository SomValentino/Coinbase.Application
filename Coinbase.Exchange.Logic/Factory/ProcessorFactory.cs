using Coinbase.Exchange.Logic.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Logic.Factory
{
    public class ProcessorFactory : IProcessorFactory
    {
        private readonly IEnumerable<ChannelProcessor> _channelProcessors;

        public ProcessorFactory(IEnumerable<ChannelProcessor> channelProcessors)
        {
            _channelProcessors = channelProcessors;
        }
        public ChannelProcessor GetProcessor(string channel)
        {
            return _channelProcessors.SingleOrDefault(x => x.Channel == channel)!;
        }
    }
}
