using Exchange.Domain;

namespace Exchange.Connectors.Bitfinex
{
    public readonly struct BitfinexSubscribeInfo
    {
        public const string TradesChannel = "trades";

        public const string CandlesChannel = "candles";

        public string Channel { get; init; }

        public int Limit { get; init; }

        public CurrencyPair Pair { get; init; }

        public int Parameter1 { get; init; }

        public int Parameter2 { get; init; }

        public int Parameter3 { get; init; }

        public static BitfinexSubscribeInfo SuscribeTrades(CurrencyPair pair, int limit = 0)
        {
            return new BitfinexSubscribeInfo
            {
                Channel = TradesChannel,
                Pair = pair,
                Limit = limit,
            };
        }

        public static BitfinexSubscribeInfo SubsribeCandles(CurrencyPair pair, int period, 
            DateTimeOffset from, DateTimeOffset to, int limit = 0)
        {
            return new BitfinexSubscribeInfo
            {
                Channel = CandlesChannel,
                Pair = pair,
                Limit = limit,
                Parameter1 = period,
                Parameter2 = from.Microsecond,
                Parameter3 = to.Microsecond
            };
        }
    }
}
