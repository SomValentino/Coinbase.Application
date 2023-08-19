using Coinbase.Exchange.SharedKernel.Models.Subscription;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace Coinbase.Exchange.API.Extensions
{
    public static class TickerDetailsExtensions
    {
        public static async Task SendToClientsAsync(this IEnumerable<TickerDetails> tickerEvents, IHubContext<ExchangeHub.ExchangeHub> hubContext)
        {
            foreach (var tickerEvent in tickerEvents)
            {
                var productId = tickerEvent.ProductId;

                var data = JsonConvert.SerializeObject(tickerEvent);

                await hubContext.Clients.Group(productId).SendAsync("receiveMarketData", data);
            }
        }
    }
}
