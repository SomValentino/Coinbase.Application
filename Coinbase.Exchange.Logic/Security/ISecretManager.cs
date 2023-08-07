using Coinbase.Exchange.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Logic.Security
{
    public interface ISecretManager
    {
        string GetSignature(string body, string secret);
        Task<Setting> GetSetting(string key);
    }
}
