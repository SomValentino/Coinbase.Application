﻿using Coinbase.Exchange.Logic.Services;
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
    public class ExchangeController : ControllerBase
    {
        private readonly ICoinbaseService _coinbaseService;

        public ExchangeController(ICoinbaseService coinbaseService)
        {
            _coinbaseService = coinbaseService;
        }

        [HttpGet("client/instruments")]
        public async Task<IActionResult> GetSubcribedInstruments()
        {
            var clientId = User.Identity?.Name;

            var instruments = await _coinbaseService.GetClientSubscribedInstrumentsAsync(clientId);

            return Ok(instruments);
        }

        [HttpGet("instruments")]
        public async Task<IActionResult> GetAllInstruments([FromQuery] InstrumentDto fetchAllInstrumentDto)
        {
            var product_ids = fetchAllInstrumentDto.ProductIds != null ?
                    string.Join(",", fetchAllInstrumentDto.ProductIds) : default;
            var queries = new Dictionary<string, string>();

            if (product_ids != null)
            {
                queries.Add("product_ids", product_ids);
            }

            if(fetchAllInstrumentDto.ContractExpiryType != null)
            {
                queries.Add("contract_expiry_type", fetchAllInstrumentDto.ContractExpiryType);
            }

            if(fetchAllInstrumentDto.ProductType != null)
            {
                queries.Add("product_type", fetchAllInstrumentDto.ProductType);
            }
                

            if (fetchAllInstrumentDto.Limit.HasValue)
            {
                queries.Add("limit", fetchAllInstrumentDto.Limit.ToString()!);
            }
            if (fetchAllInstrumentDto.Limit.HasValue)
            {
                queries.Add("offset", fetchAllInstrumentDto.Offset.ToString()!);
            }
            var instruments = await _coinbaseService.GetInstrumentsAsync(queries);
            return Ok(instruments.Products.Where(_ => !_.Is_Disabled).ToDictionary(_ => _.Product_Id));
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts([FromQuery] AccountDto accountDto)
        {
            var queries = new Dictionary<string, string>();
            if (accountDto.Limit.HasValue)
            {
                queries.Add("limit", accountDto.Limit.ToString()!);
            }
            if (accountDto.Cursor != null)
            {
                queries.Add("offset", accountDto.Cursor!);
            }

            var accounts = await _coinbaseService.GetAllAccountAsync(queries);
            return Ok(accounts!.Accounts);
        }

        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetAccount(string accountId)
        {
            var account = await _coinbaseService.GetAccountEntryAsync(accountId);
            return Ok(account?.Account);
        }
    }
}
