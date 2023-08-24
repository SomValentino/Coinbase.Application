using Coinbase.Exchange.SharedKernel.Models.ApiDto;
using Coinbase.Exchange.SharedKernel.Models.Products;
using Coinbase.Exchange.SharedKernel.Models.Subscription;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace Coinbase.Exchange.API.Extensions
{
    public static class OrderUpdateExtensions
    {
        public static async Task SendToClientsAsync(this IEnumerable<OrderBookUpdate> orderbookUpdate,
            string instrument,
            IHubContext<ExchangeHub.ExchangeHub> hubContext)
        {
            var data = JsonConvert.SerializeObject(orderbookUpdate);

            await hubContext.Clients.Group(instrument).SendAsync("receiveOrderBookUpdate", data);
        }
    }
}
