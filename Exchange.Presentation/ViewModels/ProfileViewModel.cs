using Exchange.Presentation.Commands;
using Exchange.Presentation.Models;
using Exchange.Presentation.Services;
using System.ComponentModel;

namespace Exchange.Presentation.ViewModels
{
    public class ProfileViewModel
    {
        private readonly ICurrencyConverter _converter;
        private readonly Lazy<IAsyncCommand> _convertCommand;

        public IAsyncCommand ConvertCommand => _convertCommand.Value;

        public Wallet Wallet { get; }

        public Wallet ConvertWallet { get; }

        public ProfileViewModel(ICurrencyConverter converter, Wallet wallet, Wallet convertWallet)
        {
            _converter = converter;
            _convertCommand = new Lazy<IAsyncCommand>(new AsyncCommand(ConvertAsync));

            Wallet = wallet;
            ConvertWallet = convertWallet;
        }

        private async Task ConvertAsync(CancellationToken cancellation)
        {
            foreach (var currency in ConvertWallet.Currencies)
            {
                var value = await _converter.ConvertToAsync(currency.Key, Wallet);
                ConvertWallet.SetCurrency(currency.Key, value);
            }

            ConvertWallet.NotifyChanged();
        }
    }
}