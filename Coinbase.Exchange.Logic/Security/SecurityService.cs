using Ardalis.Specification;
using Coinbase.Exchange.Domain.Entities;
using Coinbase.Exchange.Infrastructure.Repository;
using Coinbase.Exchange.SharedKernel.Models.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Coinbase.Exchange.Logic.Security
{
    public class SecurityService : ISecurityService
    {
        private readonly IRepository<ClientRegistration> _repository;
        private readonly ApiConfiguration _apiConfig;

        public SecurityService(IRepository<ClientRegistration> repository, IOptions<ApiConfiguration> apiConfig)
        {
            _repository = repository;
            _apiConfig = apiConfig.Value;
        }
        public async Task<(string tokenId, string token)> GenerateJwtFor(string clientId,string clienName, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiConfig.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenId = Guid.NewGuid().ToString();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, clientId),
                new Claim(JwtRegisteredClaimNames.Jti, tokenId),
                new Claim(JwtRegisteredClaimNames.Name, clienName),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.Role,_apiConfig.Role),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: _apiConfig.JwtIssuer,
                audience: _apiConfig.JwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(3600),
                signingCredentials: credentials);

            return (tokenId, new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task RegisterClient(ClientRegistration clientRegistration)
        {
            var client = await _repository.GetByIdAsync(clientRegistration.ClientId);

            if (client != null)
            {
                client.TokenId = clientRegistration.TokenId;
                client.UpdatedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(client);
            }
            else
            {
                await _repository.AddAsync(clientRegistration);
            }

            await _repository.SaveChangesAsync();
        }
    }
}
