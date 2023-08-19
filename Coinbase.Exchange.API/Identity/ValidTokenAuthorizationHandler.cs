using Coinbase.Exchange.Domain.Entities;
using Coinbase.Exchange.Domain.Specifications;
using Coinbase.Exchange.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace Coinbase.Exchange.API.Identity
{
    public class ValidTokenAuthorizationHandler : AuthorizationHandler<ValidTokenrequirement>
    {
        private readonly IRepository<ClientRegistration> _repository;

        public ValidTokenAuthorizationHandler(IRepository<ClientRegistration> repository)
        {
            _repository = repository;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidTokenrequirement requirement)
        {
            var tokenId = context.User.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var clientId = context.User.Identity?.Name;

            var clientRegistration = await _repository.GetBySpecAsync(new ClientRegistrationSpec(clientId, tokenId));

            if (clientRegistration != null)
            {
                context.Succeed(requirement);
            }
        }
    }
}
