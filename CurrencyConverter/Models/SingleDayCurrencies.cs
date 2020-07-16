namespace CurrencyConverter.Models
{
    using System.Collections.Generic;

    public class SingleDayCurrencies
    {
        public string Date { get; set; }
        public List<Currency> Currencies { get; set; }
    }
}
