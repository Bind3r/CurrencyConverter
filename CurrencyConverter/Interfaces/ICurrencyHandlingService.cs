namespace CurrencyConverter.Interfaces
{
    using System;
    using CurrencyConverter.Models;

    public interface ICurrencyFetchingService
    {
        bool TryGetCurrencies(DateTime? date, out SingleDayCurrencies result, out string error);
    }
}
