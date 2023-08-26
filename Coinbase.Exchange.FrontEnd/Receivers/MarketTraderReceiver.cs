using System;
using System.Collections.Generic;
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
                case "BestBid":
                    _dataStore[instrument].BestBid = data;
                    break;
                case "BestOffer":
                    _dataStore[instrument].BestOffer = data;
                    break;
                case "Offers":
                    _dataStore[instrument].Offers = data;
                    break;
                case "Bids":
                    _dataStore[instrument].Bids = data;
                    break;
                case "Price":
                    _dataStore[instrument].Price = data;
                    break;
            }

            OnMarketDataUpdate?.Invoke(this, new MarketDataEventArgs() { Store = _dataStore, Type = type });
        }
    }
}
