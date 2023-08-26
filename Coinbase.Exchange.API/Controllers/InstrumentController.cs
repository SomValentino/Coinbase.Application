using Coinbase.Exchange.Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coinbase.Exchange.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class InstrumentController : ControllerBase
    {
        private readonly ICoinbaseService _coinbaseService;

        public InstrumentController(ICoinbaseService coinbaseService)
        {
            _coinbaseService = coinbaseService;
        }

        [HttpGet("client")]
        public async Task<IActionResult> GetSubcribedInstruments()
        {
            var clientId = User.Identity?.Name;

            var instruments = await _coinbaseService.GetClientSubscribedInstrumentsAsync(clientId);

            return Ok(instruments);
        }
    }
}
