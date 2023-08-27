using Coinbase.Exchange.Logic.Services;
using Coinbase.Exchange.SharedKernel.Models.ApiDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Web;

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

        [HttpGet]
        public async Task<IActionResult> GetAllInstruments([FromQuery] InstrumentDto fetchAllInstrumentDto)
        {
            var product_ids = fetchAllInstrumentDto.ProductIds != null ?
                    string.Join(",", fetchAllInstrumentDto.ProductIds) : default;
            var queries = new Dictionary<string, string>
                {
                    { "product_ids", product_ids!},
                    { "product_type", fetchAllInstrumentDto.ProductType!},
                    { "contract_expiry_date",fetchAllInstrumentDto.ContractExpiryType!}
                };

            if (fetchAllInstrumentDto.Limit.HasValue)
            {
                queries.Add("limit", fetchAllInstrumentDto.Limit.ToString()!);
            }
            if (fetchAllInstrumentDto.Limit.HasValue)
            {
                queries.Add("offset", fetchAllInstrumentDto.Offset.ToString()!);
            }
            var accounts_params = await _coinbaseService.GetInstrumentsAsync(queries);
            return Ok(accounts_params);
        }
    }
}
