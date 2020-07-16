namespace CurrencyConverter.Interfaces
{
    using CurrencyConverter.Models;

    public interface ICurrencyCachingService
    {
        bool TryGetCurrenciesFromCache(string key, out SingleDayCurrencies result, out string error);
        bool TrySaveCurrenciesToCache(string key, SingleDayCurrencies source, out string error);
    }
}
