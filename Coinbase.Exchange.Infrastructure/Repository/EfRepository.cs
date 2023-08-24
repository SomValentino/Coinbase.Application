using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Coinbase.Exchange.Infrastructure.Data;

namespace Coinbase.Exchange.Infrastructure.Repository
{
    public class EfRepository<T> : RepositoryBase<T>, IRepository<T> where T : class
    {
        public EfRepository(ExchangeDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<T>> GetListAsync(Specification<T> specification, CancellationToken cancellationToken = default)
        {
            return await base.ListAsync(specification, cancellationToken);
        }
    }
}
