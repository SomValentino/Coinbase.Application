using Ardalis.Specification;
using Coinbase.Exchange.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Domain.Specifications
{
    public class InstrumentsByNamesSpec: Specification<Instrument>
    {
        public InstrumentsByNamesSpec(IEnumerable<string> instruments)
        {
            Query.Where(_ => instruments.Contains(_.Name))
                 .Include(_ => _.Clients);
        }
    }
}
