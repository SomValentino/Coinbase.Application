using Ardalis.Specification;
using Coinbase.Exchange.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Domain.Specifications
{
    public class ClientRegistrationSpec : Specification<ClientRegistration>
    {
        public ClientRegistrationSpec(string clientId, string tokenId)
        {
            Query.Where(_ => _.ClientId == clientId && _.TokenId == tokenId && _.Active);
        }
    }
}
