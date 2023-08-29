using Coinbase.Exchange.Logic.Builders;
using Coinbase.Exchange.Logic.DataFeed;
using Coinbase.Exchange.Logic.Factory;
using Coinbase.Exchange.Logic.Processors;
using Coinbase.Exchange.Logic.Security;
using Coinbase.Exchange.Logic.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IRequestBuilder,RequestBuilder>();
            services.AddScoped<ICoinbaseService, CoinbaseService>();
            services.AddSingleton<L2ChannelProcessor>();
            services.AddSingleton<TickerChannelProcessor>();
            services.AddSingleton<CandlesChannelProcessor>();
            services.AddSingleton<IProcessorFactory, ProcessorFactory>();
        }

       
    }
}
