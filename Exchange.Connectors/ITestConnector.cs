using Exchange.Domain;

namespace Exchange.Connectors
{
    /*ИЗМЕНЕНИЯ
    *Для Валютных пар был создан отдельная структура CurrencyPair для более гибкого парсинга валют
    *
    *Методы Subscribe и Unsubscribe были превращены в свои асинхронные версии, так как эти методы используются для взаимодействия с сетью (с web socket).
    */
    public interface ITestConnector
    {
        #region Rest

        Task<IEnumerable<Trade>> GetNewTradesAsync(CurrencyPair pair, int maxCount);
        Task<IEnumerable<Candle>> GetCandleSeriesAsync(CurrencyPair pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0);

        #endregion

        #region Socket

        event Action<Trade> NewBuyTrade;
        event Action<Trade> NewSellTrade;
        Task SubscribeTradesAsync(CurrencyPair pair, int maxCount = 100);
        Task UnsubscribeTradesAsync(CurrencyPair pair);

        event Action<Candle> CandleSeriesProcessing;
        Task SubscribeCandlesAsync(CurrencyPair pair, int periodInSec, DateTimeOffset? from = null, DateTimeOffset? to = null, long? count = 0);
        Task UnsubscribeCandlesAsync(CurrencyPair pair);

        #endregion

    }
}
