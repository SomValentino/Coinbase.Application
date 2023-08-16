﻿using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Domain.Specifications
{
    public class ClientsByProductGroupNameSpec : Specification<Domain.Entities.ProductGroup>
    {
        public ClientsByProductGroupNameSpec(string productName)
        {
            Query
                .Where(group => group.Instrument.Equals(productName, StringComparison.OrdinalIgnoreCase))
                .Include(group => group.Clients);
        }
    }
}