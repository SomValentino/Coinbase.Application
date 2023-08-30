using Coinbase.Exchange.FrontEnd.Receivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.FrontEnd.Factory
{
    public class ReceiverFactory : IReceiverFactory
    {
        private readonly IEnumerable<DataReceiver> _dataReceivers;

        public ReceiverFactory(IEnumerable<DataReceiver> dataReceivers)
        {
            _dataReceivers = dataReceivers;
        }
        public DataReceiver GetDataReceiver(string type)
        {
            return _dataReceivers.SingleOrDefault(_ => _.Type == type)!;
        }
    }
}
