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
        ICurrencyFetchingService _currencyHandlingService;

        public CurrencyController(ICurrencyFetchingService currencyHandlingService)
        {
            _currencyHandlingService = currencyHandlingService;
        }

        [HttpGet, HttpGet("{date}")]
        public IActionResult GetCurrency(DateTime? date)
        {
            if (_currencyHandlingService.TryGetCurrencies(date, out SingleDayCurrencies currencies, out string error))
            {
                return Ok(currencies);
            }

            return Problem(error);
        }
    }
}
