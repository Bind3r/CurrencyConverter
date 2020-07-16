namespace CurrencyConverterTests
{
    using System.Collections.Generic;
    using System.Linq;
    using CurrencyConverter.Models;
    using CurrencyConverter.Services;
    using CurrencyConverterTests.TestFiles;
    using NUnit.Framework;

    class CurrencyParsingServiceTests
    {
        [Test]
        public void ParseCurrenciesSource_WithProvidedCorrectSource_ReturnExpectedResults()
        {
            string source = new TestDailySource().CorrectSource;

            CurrencyParsingService parsingService = new CurrencyParsingService();
            HashSet<SingleDayCurrencies> result = parsingService.ParseCurrenciesSource(source);

            SingleDayCurrencies expected = new SingleDayCurrencies
            {
                Date = "2020-07-16",
                Currencies = new List<Currency>
                {
                    new Currency
                    {
                        Code = "USD",
                        Ratio = 1.1414m
                    }
                }
            };

            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result.First().Date, expected.Date);
            Assert.AreEqual(result.First().Currencies.Count, 1);
            Assert.AreEqual(result.First().Currencies.First().Code, expected.Currencies.First().Code);
            Assert.AreEqual(result.First().Currencies.First().Ratio, expected.Currencies.First().Ratio);
        }

        [Test]
        public void ParseCurrenciesSource_NoSourceProvided_ReturnExpectedException()
        {
            CurrencyParsingService parsingService = new CurrencyParsingService();
            Assert.That(() => parsingService.ParseCurrenciesSource(null), Throws.Exception);
        }

        [Test]
        public void ParseCurrenciesSource_WithIncorrectSource_ReturnExpectedException()
        {
            CurrencyParsingService parsingService = new CurrencyParsingService();
            string source = new TestDailySource().IncorectDailySource;
            Assert.That(() => parsingService.ParseCurrenciesSource(source), Throws.Exception);
        }

        [Test]
        public void ParseCurrenciesSource_WithNoCurrenciesSource_ShouldReturnEmptyList()
        {
            CurrencyParsingService parsingService = new CurrencyParsingService();
            string source = new TestDailySource().NoCurrenciesSource;
            HashSet<SingleDayCurrencies> result = parsingService.ParseCurrenciesSource(source);
            Assert.IsEmpty(result);
        }
    }
}
