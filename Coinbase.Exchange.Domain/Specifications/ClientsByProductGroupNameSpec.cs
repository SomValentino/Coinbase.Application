using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Domain.Specifications
{
    public class ClientsByProductGroupNameSpec : Specification<Domain.Entities.Instrument>
    {
        public ClientsByProductGroupNameSpec(string productName)
        {
            Query
                .Where(group => group.Name == productName)
                .Include(group => group.Clients);
        }
    }
}
