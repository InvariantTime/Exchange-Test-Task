using Exchange.Domain;
using System.Text.Json.Nodes;

namespace Exchange.Connectors.Bitfinex
{
    public class BitfinexConverter
    {
        public IEnumerable<Trade> ConvertTrades(CurrencyPair pair, JsonArray document)
        {
            return Enumerable.Empty<Trade>();
        }

        public IEnumerable<Candle> ConvertCandles(CurrencyPair pair, JsonArray document)
        {
            return Enumerable.Empty<Candle>();
        }

        public string ConvertSecondsPeriod(int period)
        {
            return string.Empty;
        }

        private string GetSide(float amount)
        {
            return amount > 0 ? "buy" : "sell";
        }
    }
}
