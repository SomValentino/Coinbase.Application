using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Infrastructure.Repository
{
    public interface IRepository<T> : IRepositoryBase<T> where T : class
    {
        Task<IEnumerable<T>> ListAsync(Specification<T> specification, CancellationToken cancellationToken = default);
    }
}
