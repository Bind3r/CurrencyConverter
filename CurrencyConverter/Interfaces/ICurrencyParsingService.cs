namespace CurrencyConverter.Interfaces
{
    using System.Collections.Generic;
    using CurrencyConverter.Models;

    public interface ICurrencyParsingService
    {
        HashSet<SingleDayCurrencies> ParseCurrenciesSource(string source);
    }
}
