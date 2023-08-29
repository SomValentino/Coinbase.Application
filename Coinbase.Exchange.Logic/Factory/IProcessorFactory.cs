using Coinbase.Exchange.Logic.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Logic.Factory
{
    public interface IProcessorFactory
    {
        ChannelProcessor GetProcessor(string channel);
    }
}
