using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CurrencyConversion.Models;
using CurrencyConversion.Presenter;

namespace CurrencyConversion
{
    public interface IMainWindow
    {
        MessageBoxResult DisplayError(string message);
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        private MainWindowPresenter Presenter { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Presenter = new MainWindowPresenter(this);
            DataContext = Presenter.Model;
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            Presenter.Calculate();
        }

        public MessageBoxResult DisplayError(String message)
        {
            return MessageBox.Show(this, message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
