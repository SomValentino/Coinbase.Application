using Coinbase.Exchange.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace Coinbase.Exchange.Logic.Security
{
    public class SecretManager : ISecretManager
    {
        public Task<Setting> GetSetting(string key)
        {
            throw new NotImplementedException();
        }

        public string GetSignature(string body, string secret)
        {
            var bodybytes = Encoding.UTF8.GetBytes(body);
            var secretbytes = Encoding.UTF8.GetBytes(secret);

            var hash = new HMACSHA256(secretbytes);
            var computedHash = hash.ComputeHash(bodybytes);

            return Convert.ToHexString(computedHash);
        }
    }
}
