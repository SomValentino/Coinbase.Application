using Ardalis.GuardClauses;
using Coinbase.Exchange.FrontEnd.Factory;
using Coinbase.Exchange.SharedKernel.Models.ApiDto;
using Coinbase.Exchange.SharedKernel.Models.Subscription;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.FrontEnd.Receivers
{
    public class MarketTraderReceiver
    {
        private readonly IReceiverFactory _receiverFactory;

        public MarketTraderReceiver(IReceiverFactory receiverFactory)
        {
            _receiverFactory = Guard.Against.Null(receiverFactory,nameof(receiverFactory));
        }
        public void OnReceiveOrderBookMarketData(string instrument, string type, string data)
        {
            var receiver = _receiverFactory.GetDataReceiver(type);

            Guard.Against.Null(receiver, nameof(receiver));

            receiver.ReceiveMarketData(instrument, data);
        }
    }
}
