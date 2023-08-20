using Coinbase.Exchange.Infrastructure.Data;
using Coinbase.Exchange.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Infrastructure
{
    public static class ServiceInstaller
    {
        public static void AddServiceInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        }
    }
}
