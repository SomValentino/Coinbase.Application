using Coinbase.Exchange.SharedKernel.Constants;
using Coinbase.Exchange.SharedKernel.Models.Subscription;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.FrontEnd.Receivers
{
    public class CandlesReceiver : DataReceiver
    {
        public override string Type => MessageType.Candles;

        public override void ReceiveMarketData(string instrument, string data)
        {
            TryAddInstrumentToStore(instrument);
            _dataStore[instrument].Candles.AddRange(JsonConvert.DeserializeObject<List<CandleDetails>>(data));
            UpdateEventControls();
        }
    }
}
