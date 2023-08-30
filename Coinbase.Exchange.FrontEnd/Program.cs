using Coinbase.Exchange.FrontEnd.ApiClient;
using Coinbase.Exchange.FrontEnd.Factory;
using Coinbase.Exchange.FrontEnd.Receivers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace Coinbase.Exchange.FrontEnd
{
    internal static class Program
    {
        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var host = CreateHostBuilder().Build();
            var serviceProvider = host.Services;
            
            Application.Run(serviceProvider.GetRequiredService<HomeForm>());
        }

        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    
                    services.AddTransient<BidsReceiver>();
                    services.AddTransient<OfferReceiver>();
                    services.AddTransient<PriceReceiver>();
                    services.AddTransient<CandlesReceiver>();
                    services.AddTransient<MarketTraderReceiver>();
                    services.AddTransient<MarketDataApiClient>();
                    services.AddTransient<IReceiverFactory, ReceiverFactory>();
                    
                    services.AddTransient<HomeForm>();

                    var serviceProvider = services.BuildServiceProvider();

                    services.AddTransient<IEnumerable<DataReceiver>>(options => GetInstances<DataReceiver>(serviceProvider));
                });
        }

        static List<T> GetInstances<T>(IServiceProvider serviceProvider)
        {
            var instances = new List<T>();
            var foundInstances = Assembly.GetAssembly(typeof(T))?.GetTypes()
                                ?.Where(detector => detector.IsClass &&
                                !detector.IsAbstract && typeof(T).IsAssignableFrom(detector));

            if (foundInstances != null && foundInstances.Any())
            {
                foreach (var type in foundInstances)
                {
                    var typeDetector = (T?)serviceProvider.GetService(type);
                    if (typeDetector != null)
                        instances.Add(typeDetector);
                }
            }
            return instances;
        }
    }
}