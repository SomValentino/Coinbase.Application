using Coinbase.Exchange.SharedKernel.Models.ApiDto;
using Coinbase.Exchange.SharedKernel.Models.Products;
using Coinbase.Exchange.SharedKernel.Models.Subscription;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Coinbase.Exchange.API.Extensions
{
    public static class PriceUpdateExtensions
    {
        public static async Task SendToClientsAsync(this PriceUpdate priceUpdate,
            string type,
            IHubContext<ExchangeHub.ExchangeHub> hubContext)
        {
            await hubContext.Clients.Group(priceUpdate.Instrument).SendAsync("receivePriceUpdate",type ,priceUpdate.Instrument,priceUpdate.Price);
        }
    }
}
