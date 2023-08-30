using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.FrontEnd.Receivers
{
    public abstract class DataReceiver
    {
        protected static Dictionary<string, MarketData> _dataStore = new Dictionary<string, MarketData>();

        public static event EventHandler<MarketDataEventArgs>? OnMarketDataUpdate;

        public abstract string Type { get; }
        public abstract void ReceiveMarketData(string instrument, string data);
        protected void TryAddInstrumentToStore(string instrument)
        {
            if (!_dataStore.ContainsKey(instrument))
            {
                _dataStore.Add(instrument, new MarketData());
            }
        }
        protected void UpdateEventControls() => OnMarketDataUpdate?.Invoke(this, new MarketDataEventArgs() { Store = _dataStore });
    }
}
