using Coinbase.Exchange.SharedKernel.Models.ApiDto;
using Coinbase.Exchange.SharedKernel.Models.Products;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.FrontEnd.ApiClient
{
    public class MarketDataApiClient
    {
        private readonly MemoryCache _cache;
        private HttpClient _client;

        public MarketDataApiClient()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var cachingOptions = Microsoft.Extensions.Options.Options.Create(new MemoryCacheOptions
            {
                ExpirationScanFrequency = TimeSpan.FromSeconds(3000)
            });
            _cache = new MemoryCache(cachingOptions);
        }

        public async Task<string?> GetAccessToken()
        {
            try
            {
                var token = _cache.Get("token")?.ToString();
                if (token != null)
                {
                    return token;
                }

                var data = JsonConvert.SerializeObject(new AuthRequest
                {
                    ClientId = "58e3db40ac3646699669836ae43516fb",
                    ClientName = "test"
                });

                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("http://127.0.0.1:5190/api/auth", content);

                response.EnsureSuccessStatusCode();

                token = await response.Content.ReadAsStringAsync();

                token = token.Replace("\"",string.Empty);

                _cache.Set("token", token);

                return token;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetSubscribedInstruments()
        {
            try
            {
                await AddTokenBearer();

                var response = await _client.GetStringAsync("http://127.0.0.1:5190/api/instrument/client");

                var instruments = JsonConvert.DeserializeObject<IEnumerable<string>>(response);

                return instruments;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

       
        public async Task<Dictionary<string,Product>> GetAllTradedInstruments()
        {
            try
            {
                await AddTokenBearer();

                var response = await _client.GetStringAsync("http://127.0.0.1:5190/api/instrument");

                var instruments = JsonConvert.DeserializeObject<Dictionary<string, Product>>(response);

                return instruments;
            }
            catch (Exception)
            {

                throw;
            }
        }


        private async Task AddTokenBearer()
        {
            var token = await GetAccessToken();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }


    }
}
