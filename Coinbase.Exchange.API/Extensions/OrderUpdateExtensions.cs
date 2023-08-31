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
        public static async Task SendToClientsAsync(this MarketDataResult marketData,
            IHubContext<ExchangeHub.ExchangeHub> hubContext)
        {
            await hubContext.Clients.Group(marketData.Instrument).SendAsync("MarketDataResultUpdate",
                marketData.Instrument,
                marketData.Type, marketData.Data);
        }
    }
}
