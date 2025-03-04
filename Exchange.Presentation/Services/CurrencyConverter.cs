using Exchange.Connectors;
using Exchange.Domain;
using Exchange.Presentation.Models;

namespace Exchange.Presentation.Services
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private const string _convertCurrency = "USD";

        private readonly ITestConnector _connector;

        public CurrencyConverter(ITestConnector connector)
        {
            _connector = connector;
        }

        public async Task<decimal> ConvertToAsync(string currency, Wallet other)
        {
            decimal result = 0;
            decimal usd = 0;

            foreach (var current in other.Currencies)
            {
                if (currency == current.Key)
                {
                    result += current.Value;
                    continue;
                }

                var pair = new CurrencyPair(current.Key, _convertCurrency);
                var candle = await _connector.GetCandleSeriesAsync(pair, 60, default, default, 1);

                usd += candle.FirstOrDefault()?.ClosePrice ?? 0;
            }

            if (currency == _convertCurrency)
                return result + usd;

            var resultCandles = await _connector.GetCandleSeriesAsync(new CurrencyPair(currency, _convertCurrency), 60, default, default, 1);
            var usdCandle = resultCandles.FirstOrDefault()?.ClosePrice ?? 0;

            return result + usd * (usdCandle == 0 ? 0 : 1 / usdCandle);
        }
    }
}
