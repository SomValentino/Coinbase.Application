using Coinbase.Exchange.SharedKernel.Constants;
using Coinbase.Exchange.SharedKernel.Models.ApiDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.FrontEnd.Receivers
{
    public class PriceReceiver : DataReceiver
    {
        public override string Type => MessageType.Price;

        public override void ReceiveMarketData(string instrument, string data)
        {
            TryAddInstrumentToStore(instrument);
            _dataStore[instrument].Price = decimal.Parse(data, CultureInfo.InvariantCulture);
            UpdateEventControls();
        }
    }
}
