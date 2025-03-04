using Exchange.Presentation.Services;
using Exchange.Presentation.View;
using Exchange.Presentation.ViewModels;
using Exchange.Presentation.Models;
using System.Windows;
using Exchange.Connectors.Bitfinex;

namespace Exchange.Presentation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var wallet = new Wallet();
            wallet.SetCurrency("BTC", 1);
            wallet.SetCurrency("XRP", 15000);
            wallet.SetCurrency("XMR", 50);
            wallet.SetCurrency("DASH", 30);

            var convertWallet = new Wallet();
            convertWallet.SetCurrency("USD", 0);
            convertWallet.SetCurrency("BTC", 0);
            convertWallet.SetCurrency("XRP", 0);
            convertWallet.SetCurrency("XMR", 0);
            convertWallet.SetCurrency("DASH", 0);

            var connector = BitfinexConnector.Create();
            var converter = new CurrencyConverter(connector);

            var rest = new RestViewModel(connector);
            var socket = new SocketViewModel(connector);
            var profile = new ProfileViewModel(converter, wallet, convertWallet);

            var mainViewModel = new MainViewmodel(profile, rest, socket);
            MainWindow window = new MainWindow(mainViewModel);

            window.Show();
        }
    }

}
