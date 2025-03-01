using Exchange.Domain;

namespace Exchange.Connectors.Bitfinex
{
    public readonly struct BitfinexUnsubscribeInfo
    {
        public string Channel { get; init; }

        public CurrencyPair Pair { get; init; }

        public static BitfinexUnsubscribeInfo UnsubscribeCandles(CurrencyPair pair)
        {
            return new BitfinexUnsubscribeInfo
            {
                Channel = BitfinexSubscribeInfo.CandlesChannel,
                Pair = pair
            };
        }

        public static BitfinexUnsubscribeInfo UnsubscribeTrades(CurrencyPair pair)
        {
            return new BitfinexUnsubscribeInfo
            {
                Channel = BitfinexSubscribeInfo.TradesChannel,
                Pair = pair
            };
        }
    }
}
