using Coinbase.Exchange.FrontEnd.Config;
using Coinbase.Exchange.SharedKernel.Models.Account;
using Coinbase.Exchange.SharedKernel.Models.ApiDto;
using Coinbase.Exchange.SharedKernel.Models.Products;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Coinbase.Exchange.FrontEnd.ApiClient
{
    public class MarketDataApiClient
    {
        private readonly MemoryCache _cache;
        private readonly Setting _setting;
        private HttpClient _client;

        public MarketDataApiClient(IOptions<Setting> settingOptions)
        {
            _setting = settingOptions.Value;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_setting.BaseUrl);
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
                    ClientId = _setting.ClientId,
                    ClientName = _setting.ClientName
                });

                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("/api/auth", content);

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

                var response = await _client.GetStringAsync("/api/exchange/client/instruments");

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

                var response = await _client.GetStringAsync("/api/exchange/instruments");

                var instruments = JsonConvert.DeserializeObject<Dictionary<string, Product>>(response);

                return instruments;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<AccountEntry>> GetAllAccounts()
        {
            try
            {
                await AddTokenBearer();

                var response = await _client.GetStringAsync("/api/exchange/accounts");

                var accounts = JsonConvert.DeserializeObject<List<AccountEntry>>(response);

                return accounts;
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
