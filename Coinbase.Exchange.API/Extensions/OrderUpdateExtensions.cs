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
        public static async Task SendToClientsAsync(this string orderbookUpdate,
            string instrument,
            string type,
            IHubContext<ExchangeHub.ExchangeHub> hubContext)
        {
            
            await hubContext.Clients.Group(instrument).SendAsync("receiveOrderBookUpdate",instrument,type ,orderbookUpdate);
        }
    }
}
