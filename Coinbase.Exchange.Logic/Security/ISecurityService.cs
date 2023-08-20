using Coinbase.Exchange.Domain.Entities;

namespace Coinbase.Exchange.Logic.Security
{
    public interface ISecurityService
    {
        Task RegisterClient(ClientRegistration clientRegistration);
        Task<(string tokenId, string token)> GenerateJwtFor(string clientId, string clienName, string role);
    }
}
