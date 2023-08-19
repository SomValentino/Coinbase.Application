using Coinbase.Exchange.SharedKernel.Models.Subscription;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Coinbase.Exchange.API.Extensions
{
    public static class OrderEventsExtensions
    {
        public static async Task SendToClientsAsync(this List<OrderEvent> orders, IHubContext<ExchangeHub.ExchangeHub> hubContext)
        {
            foreach (var order in orders)
            {
                var productId = order.ProductId;

                var data = JsonConvert.SerializeObject(order);

                await hubContext.Clients.Group(productId).SendAsync("receiveMarketData", data);
            }
        }
    }
}
