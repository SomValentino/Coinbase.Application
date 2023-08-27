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
        private static Dictionary<string, MarketData> _dataStore = new Dictionary<string, MarketData>();

        public static event EventHandler<MarketDataEventArgs>? OnMarketDataUpdate;


        public void OnReceiveOrderBookMarketData(string instrument, string type, string data)
        {
            if(!_dataStore.ContainsKey(instrument))
            {
                _dataStore.Add(instrument, new MarketData());
            }

            switch (type)
            {
                case "Offers":
                    _dataStore[instrument].Offers = JsonConvert.DeserializeObject<List<OrderBookUpdate>>(data);
                    break;
                case "Bids":
                    _dataStore[instrument].Bids = JsonConvert.DeserializeObject<List<OrderBookUpdate>>(data);
                    break;
                case "Price":
                    _dataStore[instrument].Price = decimal.Parse(data,CultureInfo.InvariantCulture);
                    break;
            }

            OnMarketDataUpdate?.Invoke(this, new MarketDataEventArgs() { Store = _dataStore, Type = type });
        }
    }
}
