using Coinbase.Exchange.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Infrastructure.Data
{
    public class ExchangeDbContext : DbContext
    {
        public ExchangeDbContext(DbContextOptions<ExchangeDbContext> options) : base(options)
        {
            
        }

        public DbSet<Domain.Entities.Client> Clients => Set<Domain.Entities.Client>();
        public DbSet<Domain.Entities.Instrument> productGroups => Set<Instrument>();
        public DbSet<ClientRegistration> ClientRegistrations => Set<ClientRegistration>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
