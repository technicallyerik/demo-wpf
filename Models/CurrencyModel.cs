using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CurrencyConversion.Models
{
    public class CurrencyModel : DependencyObject
    {
        public static readonly DependencyProperty CodeProperty =
            DependencyProperty.Register("Code", typeof (string), typeof (CurrencyModel), new PropertyMetadata(default(string)));

        public string Code
        {
            get { return (string) GetValue(CodeProperty); }
            set { SetValue(CodeProperty, value); }
        }

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof (string), typeof (CurrencyModel), new PropertyMetadata(default(string)));

        public string Name
        {
            get { return (string) GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
    }
}
