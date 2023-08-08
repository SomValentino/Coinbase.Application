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
            string hash;
            var encoder = new ASCIIEncoding();
            var code = encoder.GetBytes(secret);
            using (HMACSHA256 hmac = new HMACSHA256(code))
            {
                var hmBytes = hmac.ComputeHash(encoder.GetBytes(body));
                hash = ToHexString(hmBytes);
            }
            return hash;
        }

        public static string ToHexString(byte[] array)
        {
            StringBuilder hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
    }
}
