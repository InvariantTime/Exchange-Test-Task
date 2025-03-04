using Exchange.Domain;
using System.Text.Json.Nodes;

namespace Exchange.Connectors.Bitfinex
{
    public class BitfinexConverter
    {
        public IEnumerable<Trade> ConvertTrades(CurrencyPair pair, JsonArray document)
        {
            foreach (var item in document)
            {
                if (item == null)
                    continue;

                int id = (int)(item[0] ?? 0);
                long mts = (long)(item[1] ?? 0);
                decimal amount = (decimal)(item[2] ?? 0);
                decimal price = (decimal)(item[3] ?? 0);

                var side = GetSide(amount);

                yield return new Trade
                {
                    Id = id.ToString(),
                    Pair = pair,
                    Price = price,
                    Amount = Math.Abs(amount),
                    Side = side,
                    Time = BitfinexTime.ConvertTime(mts),
                };
            }
        }

        public IEnumerable<Candle> ConvertCandles(CurrencyPair pair, JsonArray document)
        {
            foreach (var item in document)
            {
                if (item == null)
                    continue;

                long mts = (long)(item[0] ?? 0);
                decimal open = (decimal)(item[1] ?? 0);
                decimal close = (decimal)(item[2] ?? 0);
                decimal high = (decimal)(item[3] ?? 0);
                decimal low = (decimal)(item[4] ?? 0);
                decimal volume = (decimal)(item[5] ?? 0);

                yield return new Candle
                {
                    Pair = pair,
                    OpenPrice = open,
                    ClosePrice = close,
                    HighPrice = high,
                    LowPrice = low,
                    TotalPrice = Math.Abs(open - close),
                    TotalVolume = volume,
                    OpenTime = BitfinexTime.ConvertTime(mts)
                };
            }
        }

        private string GetSide(decimal amount)
        {
            return amount > 0 ? "buy" : "sell";
        }
    }
}
