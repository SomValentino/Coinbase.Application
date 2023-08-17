
using Coinbase.Exchange.Logic.DataFeed;
using Microsoft.AspNetCore.SignalR;

namespace Coinbase.Exchange.API.ExchangeHub
{
    public class ExchangeHub: Hub
    {
        public ExchangeHub(ICoinbaseWebSocketClient coinbaseWebSocketClient
            )
        {
            
        }
    }
}
