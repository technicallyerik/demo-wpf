using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using CurrencyConversion.Models;
using CurrencyConversion.Services;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Windows;

namespace CurrencyConversion.Presenter
{
    public delegate Dictionary<string, string> AsyncCurrencyCaller();

    public class MainWindowPresenter
    {
        private IMainWindow View { get; set; }
        public MainWindowModel Model { get; set; }
        private ICurrencyService _currencyService;
        public ICurrencyService CurrencyService
        {
            get { return _currencyService = _currencyService ?? new CurrencyService(); }
            set { _currencyService = value; }
        }

        public MainWindowPresenter(IMainWindow view)
        {
            View = view;
            Model = new MainWindowModel();
            Model.SourceCurrencyCode = "USD";       // Initial default
            Model.DestinationCurrencyCode = "JPY";  // Initial default
            GetCurrencies();
        }


        /// <summary>
        /// Gets the currencies.  This example uses BeginInvoke with a callback to hit the service asynchronously. 
        /// </summary>
        private void GetCurrencies()
        {
            // Create and initiate the async call delegate
            var caller = new AsyncCurrencyCaller(CurrencyService.GetCurrencies);
            caller.BeginInvoke(GetCurrenciesCallback, null);
        }

        private void GetCurrenciesCallback(IAsyncResult result)
        {
            // Retrieve the delegate and results
            AsyncResult asyncResult = (AsyncResult) result;
            AsyncCurrencyCaller caller = (AsyncCurrencyCaller) asyncResult.AsyncDelegate;
            Dictionary<string, string> currencies = caller.EndInvoke(result);

            // GetCurrenciesCallback will be called in a different thread than the UI, 
            // so we need to use the dispatcher to update the dependency properties.
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)(() =>
            {
                Model.AvailableCurrencyCodes = new ObservableCollection<CurrencyModel>();
                foreach (KeyValuePair<string, string> currency in currencies)
                {
                    Model.AvailableCurrencyCodes.Add(new CurrencyModel { Code = currency.Key, Name = currency.Value });
                }
            }));
        }


        /// <summary>
        /// Calculates this exchange rate value.  This example uses async await, new to C# 5
        /// </summary>
        public async void Calculate()
        {
            // We need to retrieve a copy of these variables locally 
            // because they can't be accessed within the async task
            string sourceCurrencyCode = Model.SourceCurrencyCode;
            string destinationCurrencyCode = Model.DestinationCurrencyCode;

            decimal exchangeRate;
            try
            {
                // Create an async task and execute it
                Task<decimal> getExchangeRateTask = Task<decimal>.Factory.StartNew(() => CurrencyService.GetExchangeRate(sourceCurrencyCode, destinationCurrencyCode));

                // Retrieve the value once the async task is done
                exchangeRate = await getExchangeRateTask;
            }
            catch (CurrencyServiceException ex)
            {
                View.DisplayError(ex.Message);
                return;
            }

            Model.CalculatedAmount = Model.SourceAmount * exchangeRate;
        }
    }
}
