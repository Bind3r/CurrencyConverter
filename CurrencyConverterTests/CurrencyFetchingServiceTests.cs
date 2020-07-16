namespace CurrencyConverterTests
{
    using CurrencyConverter.Interfaces;
    using CurrencyConverter.Models;
    using CurrencyConverter.Services;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using NUnit.Framework;

    public class CurrencyFetchingServiceTests
    {
        private readonly string _dailyCurrencies = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";
        private readonly string _currenciesHistory = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist.xml";
        private readonly string _cacheKeyDateFormat = "yyyy-MM-dd";

        [Test]
        public void TryGetCurrencies_WithNoUrlsProvided_ShouldReturnError()
        {
            ILogger<CurrencyFetchingService> loggerMock = Mock.Of<ILogger<CurrencyFetchingService>>();
            ICurrencyCachingService currencyCachingService = Mock.Of<ICurrencyCachingService>();
            ICurrencyParsingService currencyParsingService = Mock.Of<ICurrencyParsingService>();
            Mock<IOptions<UrlsConfig>> urlsConfigMock = new Mock<IOptions<UrlsConfig>>();
            Mock<IOptions<CacheKeyConfig>> cacheConfigMock = new Mock<IOptions<CacheKeyConfig>>();

            urlsConfigMock.SetupGet(x => x.Value).Returns(new UrlsConfig
            {
                DailyCurrencies = null,
                CurrenciesHistory = null,
            });

            cacheConfigMock.SetupGet(x => x.Value).Returns(new CacheKeyConfig
            {
                Format = _cacheKeyDateFormat,
            });

            CurrencyFetchingService currencyFetchingService = new CurrencyFetchingService(
                currencyCachingService,
                currencyParsingService,
                loggerMock,
                urlsConfigMock.Object,
                cacheConfigMock.Object
                );

            bool result = currencyFetchingService.TryGetCurrencies(null, out SingleDayCurrencies currencies, out string error);
            Assert.IsTrue(!result);
            Assert.IsNotNull(error);
        }

        [Test]
        public void TryGetCurrencies_NoDateProvided_ShouldReturnDailyCurrencies()
        {
            ILogger<CurrencyFetchingService> loggerMock = Mock.Of<ILogger<CurrencyFetchingService>>();
            ICurrencyCachingService currencyCachingService = Mock.Of<ICurrencyCachingService>();
            ICurrencyParsingService currencyParsingService = new CurrencyParsingService();
            Mock<IOptions<UrlsConfig>> urlsConfigMock = new Mock<IOptions<UrlsConfig>>();
            Mock<IOptions<CacheKeyConfig>> cacheConfigMock = new Mock<IOptions<CacheKeyConfig>>();

            urlsConfigMock.SetupGet(x => x.Value).Returns(new UrlsConfig
            {
                DailyCurrencies = _dailyCurrencies,
                CurrenciesHistory = null,
            });

            cacheConfigMock.SetupGet(x => x.Value).Returns(new CacheKeyConfig
            {
                Format = _cacheKeyDateFormat,
            });

            CurrencyFetchingService currencyFetchingService = new CurrencyFetchingService(
                currencyCachingService,
                currencyParsingService,
                loggerMock,
                urlsConfigMock.Object,
                cacheConfigMock.Object
                );

            bool result = currencyFetchingService.TryGetCurrencies(null, out SingleDayCurrencies currencies, out string error);
            Assert.IsTrue(result);
            Assert.IsNull(error);
            Assert.IsTrue(!string.IsNullOrEmpty(currencies.Date));
        }
    }
}