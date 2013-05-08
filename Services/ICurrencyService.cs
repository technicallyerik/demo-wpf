using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CurrencyConversion.Services
{
    public interface ICurrencyService
    {
        Dictionary<string, string> GetCurrencies();
        decimal GetExchangeRate(string sourceCurrency, string destinationCurrency);
    }
}