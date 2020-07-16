namespace CurrencyConverter.Services
{
    using System.Transactions;
    using CurrencyConverter.Interfaces;
    using CurrencyConverter.Models;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;

    public class CurrencyCacheHandlingService : ICurrencyCachingService
    {
        private readonly IMemoryCache _cache;

        public CurrencyCacheHandlingService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool TryGetCurrenciesFromCache(string key, out SingleDayCurrencies result, out string error)
        {
            error = null;
            result = null;

            if (string.IsNullOrEmpty(key))
            {
                error = "No key provided";
                return false;
            }

            return _cache.TryGetValue(key, out result);
        }

        public bool TrySaveCurrenciesToCache(string key, SingleDayCurrencies source, out string error)
        {
            error = null;

            if (string.IsNullOrEmpty(key))
            {
                error = "No key provided";
                return false;
            }

            if (_cache.TryGetValue(key, out SingleDayCurrencies cachedCurrencies))
            {
                return true;
            }

            _cache.Set(key, source);
            return true;
        }
    }
}
