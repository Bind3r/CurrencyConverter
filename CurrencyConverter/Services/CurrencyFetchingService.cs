namespace CurrencyConverter.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using CurrencyConverter.Interfaces;
    using CurrencyConverter.Models;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using RestSharp;

    public class CurrencyFetchingService : ICurrencyFetchingService
    {
        private readonly ICurrencyCachingService _currencyCachingService;
        private readonly ICurrencyParsingService _currencyParsingService;
        private readonly ILogger<CurrencyFetchingService> _logger;
        private readonly UrlsConfig _urlsConfig;
        private readonly CacheKeyConfig _cacheKeyConfig;

        public CurrencyFetchingService(
            ICurrencyCachingService currencyCachingService,
            ICurrencyParsingService currencyParsingService,
            ILogger<CurrencyFetchingService> logger,
            IOptions<UrlsConfig> urlsConfig,
            IOptions<CacheKeyConfig> cacheKeyConfig)
        {
            _currencyCachingService = currencyCachingService;
            _currencyParsingService = currencyParsingService;
            _logger = logger;
            _urlsConfig = urlsConfig.Value;
            _cacheKeyConfig = cacheKeyConfig.Value;
        }

        public bool TryGetCurrencies(DateTime? date, out SingleDayCurrencies result, out string error)
        {
            error = null;
            result = new SingleDayCurrencies();

            if (date == null)
            {
                if (!TryGetDailyCurrencies(out result, out error))
                {
                    return false;
                }
            }
            else
            {
                if (!TryGetOldCurrencies((DateTime)date, out result, out error))
                {
                    return false;
                }
            }

            AddEurCurrency(result);
            return true;
        }

        private void AddEurCurrency(SingleDayCurrencies result)
        {
            if (result == null || !result.Currencies.Any() || result.Currencies.Any(x => x.Code.Equals("EUR", StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            result.Currencies.Insert(0, new Currency
            {
                Code = "EUR",
                Ratio = 1
            });
        }

        private bool TryGetDailyCurrencies(out SingleDayCurrencies result, out string error)
        {
            result = null;
            error = null;
            RestRequest requestDetails = new RestRequest(Method.GET);
            string currenciesSource = null;

            try
            {
                currenciesSource = LoadCurrenciesSource(_urlsConfig.DailyCurrencies, requestDetails);
            }
            catch (Exception e)
            {
                error = "Failed to load daily currencies";
                _logger.LogWarning(error, e);
                return false;
            }

            try
            {
                result = _currencyParsingService.ParseCurrenciesSource(currenciesSource).First();
            }
            catch (Exception e)
            {
                error = "Failed to parse daily currencies";
                _logger.LogError(error, e);
                return false;
            }

            return true;
        }

        private bool TryGetOldCurrencies(DateTime date, out SingleDayCurrencies result, out string error)
        {
            result = null;
            error = null;
            RestRequest requestDetails = new RestRequest(Method.GET);
            string currenciesSource = null;
            string key = date.ToString(_cacheKeyConfig.Format, CultureInfo.InvariantCulture);

            try
            {
                if (_currencyCachingService.TryGetCurrenciesFromCache(key, out result, out error))
                {
                    return true;
                }

                currenciesSource = LoadCurrenciesSource(_urlsConfig.CurrenciesHistory, requestDetails);
            }
            catch (Exception e)
            {
                error = "Failed to load currencies history";
                _logger.LogError(error, e);
                return false;
            }

            try
            {
                HashSet<SingleDayCurrencies> parsedCurrencies = _currencyParsingService.ParseCurrenciesSource(currenciesSource);
                foreach (SingleDayCurrencies singleDay in parsedCurrencies)
                {
                    if (!_currencyCachingService.TrySaveCurrenciesToCache(singleDay.Date, singleDay, out error))
                    {
                        _logger.LogError(error);
                    }
                }

                if (_currencyCachingService.TryGetCurrenciesFromCache(key, out result, out error))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                error = "Failed to parse currencies history";
                _logger.LogError(error, e);
                return false;
            }

            return true;
        }

        private string LoadCurrenciesSource(string url, RestRequest requestDetails)
        {
            RestClient client = new RestClient(url);
            client.Timeout = -1;
            IRestResponse response = client.Execute(requestDetails);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Content;
            }

            throw new Exception("Failed Request");
        }
    }
}
