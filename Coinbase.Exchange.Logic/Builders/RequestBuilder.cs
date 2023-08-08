using Coinbase.Exchange.Logic.Security;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Coinbase.Exchange.Logic.Builders
{
    public class RequestBuilder : IRequestBuilder
    {
        private readonly HttpClient _httpClient;
        private readonly ISecretManager _secretManager;

        public RequestBuilder(IHttpClientFactory factory, 
            ISecretManager secretManager)
        {
            _httpClient = factory.CreateClient("coinbaseapp");
            _secretManager = secretManager;
        }
        public IRequestBuilder AddCustomHeaders(Dictionary<string, string> customHeaders)
        {
            foreach (var pair in customHeaders)
            {
                _httpClient.DefaultRequestHeaders.Add(pair.Key, pair.Value);
            }
            return this;
        }

        public IRequestBuilder AddQueryParameters(Dictionary<string, string> queryParameters)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);

            foreach (var pair in queryParameters)
            {
                parameters[pair.Key] = pair.Value;
            }

            UriBuilder.Query = parameters.ToString();

            _httpClient.BaseAddress = UriBuilder.Uri;
            
            return this;
        }

        public IRequestBuilder AddResource(string resource)
        {
            UriBuilder.Path +=  resource;

            _httpClient.BaseAddress = UriBuilder.Uri;

            return this;
        }

        public async Task<TResponse?> GetAsync<TResponse>()
        {
            string path = UriBuilder.Path;

            await AddSecurityHeaders(path, "GET");

            var response = await _httpClient.GetAsync(string.Empty);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error occured");
            }

            var data = JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());

            return data;
        }

       

        public async Task<TResponse?> PostAysnc<TRequest, TResponse>(TRequest body)
        {
            var dataString = JsonConvert.SerializeObject(body);
            var content = new StringContent(dataString,Encoding.UTF8,"application/json");
            var path = UriBuilder.Path;
            await AddSecurityHeaders(path, "POST", dataString);

            var response = await _httpClient.PostAsync(string.Empty, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error occured");
            }

            var data = JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());

            return data;

        }
        private UriBuilder UriBuilder => new UriBuilder(_httpClient.BaseAddress!);
        private async Task AddSecurityHeaders(string path,string method, string body="")
        {
            var apiKey = await _secretManager.GetSetting("apiKey");
            var apiSecret = await _secretManager.GetSetting("apiSecret");

            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            var payload = $"{timestamp}{method}{path}{body}";

            var signature = _secretManager.GetSignature(payload, apiSecret.Value!);

            _httpClient.DefaultRequestHeaders.Add("CB-ACCESS-KEY", apiKey.Value!);
            _httpClient.DefaultRequestHeaders.Add("CB-ACCESS-TIMESTAMP", timestamp.ToString());
            _httpClient.DefaultRequestHeaders.Add("CB-ACCESS-SIGN", signature);
        }
    }
}
