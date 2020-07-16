namespace CurrencyConverter.Services
{
    using System.Collections.Generic;
    using System.Xml;
    using CurrencyConverter.Interfaces;
    using CurrencyConverter.Models;

    public class CurrencyParsingService : ICurrencyParsingService
    {
        public HashSet<SingleDayCurrencies> ParseCurrenciesSource(string source)
        {
            HashSet<SingleDayCurrencies> results = new HashSet<SingleDayCurrencies>();

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(source);

            XmlNamespaceManager manager = new XmlNamespaceManager(xDoc.NameTable);
            manager.AddNamespace("ns", "http://www.gesmes.org/xml/2002-08-01");
            manager.AddNamespace("ns2", "http://www.ecb.int/vocabulary/2002-08-01/eurofxref");

            XmlNodeList calendar = xDoc.SelectNodes("ns:Envelope/ns2:Cube/ns2:Cube", manager);
            foreach (XmlNode singleDay in calendar)
            {
                string date = singleDay.Attributes["time"].Value;
                SingleDayCurrencies singleDayCurrencies = new SingleDayCurrencies
                {
                    Date = date,
                    Currencies = new List<Currency>()
                };

                foreach (XmlNode singleCurrency in singleDay)
                {
                    decimal parsedRatio = XmlConvert.ToDecimal(singleCurrency.Attributes["rate"].Value);
                    singleDayCurrencies.Currencies.Add(new Currency
                    {
                        Code = singleCurrency.Attributes["currency"].Value,
                        Ratio = parsedRatio,
                    });
                }

                results.Add(singleDayCurrencies);
            }

            return results;
        }
    }
}
