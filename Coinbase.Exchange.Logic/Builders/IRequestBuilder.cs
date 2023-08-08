using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Logic.Builders
{
    public interface IRequestBuilder
    {
        IRequestBuilder AddCustomHeaders(Dictionary<string, string> customHeaders);
        IRequestBuilder AddQueryParameters(Dictionary<string, string> queryParameters);
        IRequestBuilder AddResource(string resource);
        Task<TResponse?> GetAsync<TResponse>();
        Task<TResponse?> PostAysnc<TRequest, TResponse>(TRequest body);
    }
}
