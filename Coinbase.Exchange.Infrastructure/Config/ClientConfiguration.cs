using Coinbase.Exchange.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Infrastructure.Config
{
    public class ClientConfiguration : IEntityTypeConfiguration<Domain.Entities.Client>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Client> builder)
        {
            builder
                .HasMany(_ => _.ProductGroups)
                .WithMany(_ => _.Clients);
                

            builder
                .HasOne(_ => _.ClientRegistration)
                .WithOne(_ => _.Client)
                .HasForeignKey<ClientRegistration>(_ => _.ClientId);
        }
    }
}
