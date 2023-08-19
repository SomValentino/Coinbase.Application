using Coinbase.Exchange.Logic.Builders;
using Coinbase.Exchange.Logic.DataFeed;
using Coinbase.Exchange.Logic.Security;
using Coinbase.Exchange.Logic.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Logic
{
    public static class ServiceInstaller
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IMarketDataQueue, MarketDataQueue>();
            services.AddSingleton<ICoinbaseWebSocketClient, CoinbaseWebSocketClient>();
            services.AddScoped<ISecretManager, SecretManager>();
            services.AddScoped<IRequestBuilder,RequestBuilder>();
            services.AddScoped<ICoinbaseService, CoinbaseService>();
        }
    }
}
