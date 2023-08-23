using Ardalis.Specification;
using Coinbase.Exchange.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Domain.Specifications
{
    public class ProductGroupsByClientIdSpec : Specification<Domain.Entities.Client>
    {
        public ProductGroupsByClientIdSpec(string clientId)
        {
            Query.Where(client => client.ClientId == clientId)
                 .Include(client => client.ProductGroups);
        }
    }
}
