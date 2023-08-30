using Coinbase.Exchange.SharedKernel.Models.ApiDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.FrontEnd.Receivers
{
    public class BidsReceiver : DataReceiver
    {
        public override string Type => "Bids";

        public override void ReceiveMarketData(string instrument, string data)
        {
            TryAddInstrumentToStore(instrument);
            _dataStore[instrument].Bids = JsonConvert.DeserializeObject<List<OrderBookUpdate>>(data);
            UpdateEventControls();
        }
    }
}
