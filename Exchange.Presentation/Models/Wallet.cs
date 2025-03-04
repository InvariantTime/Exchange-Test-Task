using Exchange.Presentation.Utils;
using System.Collections.ObjectModel;

namespace Exchange.Presentation.Models
{
    public class Wallet
    {
        private readonly ObservableDictionary<string, decimal> _currencies;

        public ReadOnlyObservableDictionary<string, decimal> Currencies { get; }

        public Wallet()
        {
            _currencies = new();
            Currencies = new ReadOnlyObservableDictionary<string, decimal>(_currencies);
        }

        public void SetCurrency(string currency, decimal value)
        {
            _currencies[currency] = value;
        }

        public void Clear()
        {
            _currencies.Clear();
        }

        public void AddToCurrency(string currency, decimal value)
        {
            if (_currencies.ContainsKey(currency) == false)
            {
                _currencies.Add(currency, value);
                return;
            }

            _currencies[currency] += value;
        }

        public void NotifyChanged()
        {
            _currencies.NotifyChanged();
        }
    }
}
