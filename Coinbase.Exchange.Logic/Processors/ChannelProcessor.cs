using Coinbase.Exchange.SharedKernel.Models.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Logic.Processors
{
    public abstract class ChannelProcessor
    {
        public abstract string Channel { get; }
        public abstract IEnumerable<MarketDataResult> Process(string message);

    }
}
