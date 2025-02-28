using Exchange.Domain;
using System.Text.Json.Nodes;

namespace Exchange.Connectors.Bitfinex
{
    public class BitfinexConverter
    {
        private static readonly DateTimeOffset _unixOffset = new(1970, 1, 1, 0, 0, 0, default);

        public IEnumerable<Trade> ConvertTrades(CurrencyPair pair, JsonArray document)
        {
            foreach (var item in document)
            {
                if (item == null)
                    continue;

                int id = (int)(item[0] ?? 0);
                int mts = (int)(item[1] ?? 0);
                decimal price = (decimal)(item[2] ?? 0);
                decimal amount = (decimal)(item[2] ?? 0);

                var side = GetSide(amount);

                yield return new Trade
                {
                    Id = id.ToString(),
                    Pair = pair,
                    Price = price,
                    Amount = Math.Abs(amount),
                    Side = side,
                    Time = ConvertTime(mts),
                };
            }
        }

        public IEnumerable<Candle> ConvertCandles(CurrencyPair pair, JsonArray document)
        {
            foreach (var item in document)
            {
                if (item == null)
                    continue;

                int mts = (int)(item[0] ?? 0);
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
                    OpenTime = ConvertTime(mts)
                };
            }
        }

        public string ConvertSecondsPeriod(int period)
        {
            var symbol = period switch
            {
                60 => "1m",
                300 => "5m",
                900 => "15m",
                1800 => "30m",
                3600 => "1h",
                10800 => "3h",
                21600 => "6h",
                43200 => "12h",
                86400 => "1D",
                604800 => "1W",
                1209600 => "14D",
                > 1209600 => "1M",
                _ => throw new NotSupportedException()
            };

            return symbol;
        }

        private string GetSide(decimal amount)
        {
            return amount > 0 ? "buy" : "sell";
        }

        private DateTimeOffset ConvertTime(int mts)
        {
            return _unixOffset.AddMilliseconds(mts);
        }
    }
}
