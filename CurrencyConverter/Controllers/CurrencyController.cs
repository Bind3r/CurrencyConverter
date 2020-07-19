namespace CurrencyConverter.Controllers
{
    using System;
    using CurrencyConverter.Interfaces;
    using CurrencyConverter.Models;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        ICurrencyFetchingService _currencyFetchingService;

        public CurrencyController(ICurrencyFetchingService currencyFetchingService)
        {
            _currencyFetchingService = currencyFetchingService;
        }

        [HttpGet, HttpGet("{date}")]
        public IActionResult GetCurrency(DateTime? date)
        {
            if (_currencyFetchingService.TryGetCurrencies(date, out SingleDayCurrencies currencies, out string error))
            {
                return Ok(currencies);
            }

            return Problem(error);
        }
    }
}
