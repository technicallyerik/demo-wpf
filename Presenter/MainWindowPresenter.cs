using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyConversion.Models;
using CurrencyConversion.Services;

namespace CurrencyConversion.Presenter
{
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

        private void GetCurrencies()
        {
            Model.AvailableCurrencyCodes = new ObservableCollection<CurrencyModel>();
            Dictionary<string, string> currencies = CurrencyService.GetCurrencies();
            foreach (KeyValuePair<string, string> currency in currencies)
            {
                Model.AvailableCurrencyCodes.Add(new CurrencyModel { Code = currency.Key, Name = currency.Value});
            }
        }

        public void Calculate()
        {
            decimal exchangeRate;
            try
            {
                exchangeRate = CurrencyService.GetExchangeRate(Model.SourceCurrencyCode, Model.DestinationCurrencyCode);
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
