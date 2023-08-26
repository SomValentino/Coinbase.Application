using Coinbase.Exchange.Domain.Entities;
using Coinbase.Exchange.Logic.Security;
using Coinbase.Exchange.SharedKernel.Models.ApiDto;
using Coinbase.Exchange.SharedKernel.Models.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Coinbase.Exchange.API.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly ApiConfiguration _apiConfiguration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ISecurityService securityService, 
            IOptions<ApiConfiguration> apiConfiguration,
            ILogger<AuthController> logger)
        {
            _securityService = securityService;
            _apiConfiguration = apiConfiguration.Value;
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] AuthRequest authRequest)
        {
            var role = _apiConfiguration.Role;
            _logger.LogInformation("Generating token for clientId: {id}", authRequest.ClientId);
            var (tokenId, token) = await _securityService.GenerateJwtFor(authRequest.ClientId,authRequest.ClientName, role);
            _logger.LogInformation("Obtained token with value: {value}", token);

            _logger.LogInformation("Registering client with name: {name}", authRequest.ClientName);

            var clientRegistration = new ClientRegistration
                {
                    ClientId = authRequest.ClientId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    TokenId = tokenId,
                };

            clientRegistration.Client = new Domain.Entities.Client
            {
                ClientId = authRequest.ClientId,
                ClientName = authRequest.ClientName,
            };

            await _securityService.RegisterClient(clientRegistration);
            _logger.LogInformation("Successfully registered client");

            return Ok(token);
        }

    }
}
