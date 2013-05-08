using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CurrencyConversion.Services
{
    /// <summary>
    /// A service to communicate with Open Exchange Rates
    /// </summary>
    public class CurrencyService : ICurrencyService
    {
        /// <summary>
        /// Performs the base request, and deserializes the result to a JObject.
        /// </summary>
        /// <param name="endpoint">The part after openexchangerates.org/api/</param>
        /// <param name="parameters">Dictionary of query string parameters.</param>
        /// <returns></returns>
        protected internal virtual JObject BaseRequest(string endpoint, Dictionary<string, string> parameters)
        {
            parameters = parameters ?? new Dictionary<string, string>();
            parameters.Add("app_id", CurrencyServiceSettings.Settings.Key);
            var jsonString = GetJsonFromWebService(endpoint, parameters);
            var result = (JObject)JsonConvert.DeserializeObject(jsonString);
            return result;
        }

        /// <summary>
        /// Gets a JSON string from a web service.
        /// </summary>
        /// <param name="endpoint">The full URL of the endpoint to hit.</param>
        /// <param name="parameters">The dictionary of query string parameters.</param>
        /// <returns></returns>
        protected internal virtual string GetJsonFromWebService(string endpoint, Dictionary<string, string> parameters)
        {
            var webClient = new WebClient();
            string queryString = parameters.ToQueryString();
            string jsonString = String.Empty;
            try
            {
                jsonString = webClient.DownloadString(string.Format("{0}{1}{2}", CurrencyServiceSettings.Settings.Url, endpoint, queryString));
            }
            catch (WebException ex)
            {
                // Open Exchange Rates sends error HTTP status codes in addition to error JSON.  
                // We want to ignore the exceptions thrown, and get the underlying JSON.
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    if (ex.Response != null)
                    {
                        var responseStream = ex.Response.GetResponseStream();

                        if (responseStream != null)
                        {
                            using (var reader = new StreamReader(responseStream))
                            {
                                jsonString = reader.ReadToEnd();
                            }
                        }
                    }
                }
                else
                {
                    // Error is legit.  Re-throw it.
                    throw;
                }
            }
            return jsonString;
        }

        /// <summary>
        /// Gets a dictionary of all the currency codes and their full names.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetCurrencies()
        {
            var result = new Dictionary<string, string>();
            JObject currencies = BaseRequest("currencies.json", null);
            foreach (KeyValuePair<string, JToken> currency in currencies)
            {
                result.Add(currency.Key, currency.Value.Value<string>());
            }
            return result;
        }

        /// <summary>
        /// Gets the exchange rate.
        /// </summary>
        /// <param name="sourceCurrency">The source currency.</param>
        /// <param name="destinationCurrency">The destination currency.</param>
        /// <returns></returns>
        public virtual decimal GetExchangeRate(string sourceCurrency, string destinationCurrency)
        {
            var parameters = new Dictionary<string, string> {{"base", sourceCurrency}};
            JObject exchangeData = BaseRequest("latest.json", parameters);

            JToken error = exchangeData.SelectToken("error");
            if (error != null && error.Value<bool>() == true)
            {
                JToken errorMessage = exchangeData.SelectToken("description");
                throw new CurrencyServiceException(string.Format("Currency Service Error: {0}", errorMessage.Value<string>()));
            }

            JToken rates = exchangeData.SelectToken("rates");
            JToken rate = rates.SelectToken(destinationCurrency);
            return rate.Value<decimal>();
        }
    }

    /// <summary>
    /// A custom exception type that we can test for.
    /// </summary>
    public class CurrencyServiceException : ApplicationException
    {
        public CurrencyServiceException(string message) : base(message) { }
    }
}