using Exchange.Domain;

namespace Exchange.Connectors
{
    interface ITestConnector
    {
        #region Rest

        Task<IEnumerable<Trade>> GetNewTradesAsync(CurrencyPair pair, int maxCount);
        Task<IEnumerable<Candle>> GetCandleSeriesAsync(CurrencyPair pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0);

        #endregion

        #region Socket


        event Action<Trade> NewBuyTrade;
        event Action<Trade> NewSellTrade;
        void SubscribeTrades(CurrencyPair pair, int maxCount = 100);
        void UnsubscribeTrades(CurrencyPair pair);

        event Action<Candle> CandleSeriesProcessing;
        void SubscribeCandles(CurrencyPair pair, int periodInSec, DateTimeOffset? from = null, DateTimeOffset? to = null, long? count = 0);
        void UnsubscribeCandles(CurrencyPair pair);

        #endregion

    }
}
