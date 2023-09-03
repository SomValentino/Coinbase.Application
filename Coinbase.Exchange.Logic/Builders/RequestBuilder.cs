using Coinbase.Exchange.Logic.Security;
using Coinbase.Exchange.SharedKernel.Constants;
using Coinbase.Exchange.SharedKernel.Models.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using System.Web;

namespace Coinbase.Exchange.Logic.Builders
{
    public class RequestBuilder : IRequestBuilder
    {
        private readonly HttpClient _httpClient;
        private readonly ISecretManager _secretManager;
        private readonly ApiConfiguration _apiConfig;

        public RequestBuilder(IHttpClientFactory factory, 
            ISecretManager secretManager,
            IOptions<ApiConfiguration> apiConfig)
        {
            _httpClient = factory.CreateClient("coinbaseapp");
            _secretManager = secretManager;
            _apiConfig = apiConfig.Value;
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

            var builder = new UriBuilder(UriBuilder.Uri);

            builder.Query += parameters.ToString();

            _httpClient.BaseAddress = builder.Uri;
            
            return this;
        }

        public IRequestBuilder AddResource(string resource)
        {
            var builder = new UriBuilder(UriBuilder.Uri);

            if (builder.Path.EndsWith("/"))
            {
                builder.Path += resource;
            }
            else
            {
                builder.Path +=  $"/{resource}";
            }

            

            _httpClient.BaseAddress = builder.Uri;

            return this;
        }

        public async Task<TResponse?> GetAsync<TResponse>()
        {
            string path = _httpClient.BaseAddress!.PathAndQuery.Split('?')[0];

            await AddSecurityHeaders(path, "GET");

            var response = await _httpClient.GetAsync(string.Empty);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error occured");
            }

            var datastring = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<TResponse>(datastring);

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
            var apiKey = _apiConfig.Apikey;
            var apiSecret = _apiConfig.ApiSecret;

            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            var payload = $"{timestamp}{method}{path}{body}";

            var signature = _secretManager.GetSignature(payload, apiSecret);

            _httpClient.DefaultRequestHeaders.Add(Headers.KEY, apiKey);
            _httpClient.DefaultRequestHeaders.Add(Headers.TIMESTAMP, timestamp.ToString());
            _httpClient.DefaultRequestHeaders.Add(Headers.SIGNATURE, signature);
        }
    }
}
