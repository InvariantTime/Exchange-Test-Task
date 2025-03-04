using Exchange.Domain;
using System.Text.Json;
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

                long id = (long)(item[0] ?? 0);
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

        public Trade ConvertTrade(CurrencyPair pair, JsonElement element)
        {
            long id = element[0].GetInt64();
            long mts = element[1].GetInt64();
            decimal amount = element[2].GetDecimal();
            decimal price = element[3].GetDecimal();

            var side = GetSide(amount);

            return new Trade
            {
                Id = id.ToString(),
                Pair = pair,
                Price = price,
                Amount = Math.Abs(amount),
                Side = side,
                Time = BitfinexTime.ConvertTime(mts),
            };
        }

        public Candle ConvertCandle(CurrencyPair pair, JsonElement element)
        {
            long mts = element[0].GetInt64();
            decimal open = element[1].GetDecimal();
            decimal close = element[2].GetDecimal();
            decimal high = element[3].GetDecimal();
            decimal low = element[4].GetDecimal();
            decimal volume = element[5].GetDecimal();

            return new Candle
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

        private string GetSide(decimal amount)
        {
            return amount > 0 ? "buy" : "sell";
        }
    }
}
