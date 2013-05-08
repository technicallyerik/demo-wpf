using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CurrencyConversion.Models
{
    public class MainWindowModel : DependencyObject, IDataErrorInfo, INotifyPropertyChanged
    {
        public static readonly DependencyProperty SourceAmountProperty =
            DependencyProperty.Register("SourceAmount", typeof (decimal), typeof (MainWindowModel), new PropertyMetadata(default(decimal)));

        public decimal SourceAmount
        {
            get { return (decimal)GetValue(SourceAmountProperty); }
            set { SetValue(SourceAmountProperty, value); }
        }

        public static readonly DependencyProperty CalculatedAmountProperty =
            DependencyProperty.Register("CalculatedAmount", typeof (decimal), typeof (MainWindowModel), new PropertyMetadata(default(decimal)));

        public decimal CalculatedAmount
        {
            get { return (decimal)GetValue(CalculatedAmountProperty); }
            set { SetValue(CalculatedAmountProperty, value); }
        }

        public static readonly DependencyProperty SourceCurrencyCodeProperty =
            DependencyProperty.Register("SourceCurrencyCode", typeof(string), typeof(MainWindowModel), new PropertyMetadata(default(string)));

        public string SourceCurrencyCode
        {
            get { return (string)GetValue(SourceCurrencyCodeProperty); }
            set { SetValue(SourceCurrencyCodeProperty, value); }
        }

        public static readonly DependencyProperty DestinationCurrencyCodeProperty =
            DependencyProperty.Register("DestinationCurrencyCode", typeof(string), typeof(MainWindowModel), new PropertyMetadata(default(string)));

        public string DestinationCurrencyCode
        {
            get { return (string)GetValue(DestinationCurrencyCodeProperty); }
            set { SetValue(DestinationCurrencyCodeProperty, value); }
        }

        public static readonly DependencyProperty AvailableCurrencyCodesProperty =
            DependencyProperty.Register("AvailableCurrencyCodes", typeof (ObservableCollection<CurrencyModel>), typeof (MainWindowModel), new PropertyMetadata(default(ObservableCollection<CurrencyModel>)));

        public ObservableCollection<CurrencyModel> AvailableCurrencyCodes
        {
            get { return (ObservableCollection<CurrencyModel>) GetValue(AvailableCurrencyCodesProperty); }
            set { SetValue(AvailableCurrencyCodesProperty, value); }
        }

        #region Validation

        public string Error
        {
            get
            {
                // Class-wide validation would go here
                return null;
            }
        }

        public string this[string name]
        {
            get
            {
                string result = null;
                if (name == "SourceAmount")
                {
                    if (SourceAmount <= 0)
                    {
                        result = "Source amount must be greater than zero.";
                    }
                }
                return result;
            }
        }

        #endregion

        #region Manual Property Notification

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
