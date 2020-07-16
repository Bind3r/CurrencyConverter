namespace CurrencyConverterTests
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using CurrencyConverter.Models;
    using CurrencyConverter.Services;
    using Microsoft.AspNetCore.DataProtection.KeyManagement;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using NUnit.Framework;

    public class CurrencyCacheHandlingServiceTests
    {
        [Test]
        public void SaveCurrenciesToCache_AndTryGetCurrenciesFromCache_ShouldReturnExpectedResult()
        {
            MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
            CurrencyCacheHandlingService currencyCacheHandlingService = new CurrencyCacheHandlingService(memoryCache);

            string key = "testKey";
            SingleDayCurrencies expected = new SingleDayCurrencies
            {
                Date = "2020-07-16",
                Currencies = new List<Currency>
                {
                    new Currency
                    {
                        Code = "TEST",
                        Ratio = 1
                    }
                }
            };

            bool saveCacheStatus = currencyCacheHandlingService.TrySaveCurrenciesToCache(key, expected, out string error);
            Assert.IsTrue(saveCacheStatus);
            Assert.IsNull(error);

            bool getCacheStatus = currencyCacheHandlingService.TryGetCurrenciesFromCache(key, out SingleDayCurrencies result, out error);

            Assert.IsTrue(getCacheStatus);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SaveCurrenciesToCache_WithNoKeyProvided_ShouldReturnError()
        {
            MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
            CurrencyCacheHandlingService currencyCacheHandlingService = new CurrencyCacheHandlingService(memoryCache);

            SingleDayCurrencies source = new SingleDayCurrencies
            {
                Date = "2020-07-16",
                Currencies = new List<Currency>
                {
                    new Currency
                    {
                        Code = "TEST",
                        Ratio = 1
                    }
                }
            };

            string key = "";
            bool saveCacheStatus = currencyCacheHandlingService.TrySaveCurrenciesToCache(key, source, out string error);
            Assert.IsFalse(saveCacheStatus);
            Assert.IsNotNull(error);
        }
    }
}
