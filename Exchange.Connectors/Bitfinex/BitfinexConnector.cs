using Exchange.Domain;

namespace Exchange.Connectors.Bitfinex
{
    public class BitfinexConnector : ITestConnector, IDisposable
    {
        private readonly BitfinexRestClient _restClient;
        private readonly BitfinexSocketClient _socketClient;

        public event Action<Trade>? NewBuyTrade;
        public event Action<Trade>? NewSellTrade;
        public event Action<Candle>? CandleSeriesProcessing;

        private BitfinexConnector(BitfinexRestClient rest, BitfinexSocketClient socket)
        {
            _restClient = rest;
            _socketClient = socket;
            _socketClient.CandleExecuted += CandleExecuted;
            _socketClient.TradeExecuted += TradesExecuted;
        }

        public async Task<IEnumerable<Candle>> GetCandleSeriesAsync(CurrencyPair pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0)
        {
            var candles = await _restClient.GetCandlesAsync(pair, periodInSec, from.GetValueOrDefault(), to.GetValueOrDefault());

            return candles.Take((int)(count ?? 0));
        }

        public async Task<IEnumerable<Trade>> GetNewTradesAsync(CurrencyPair pair, int maxCount)
        {
            var trades = await _restClient.GetTradesAsync(pair, maxCount);
            return trades;
        }

        public Task SubscribeCandlesAsync(CurrencyPair pair, int periodInSec, DateTimeOffset? from = null, DateTimeOffset? to = null, long? count = 0)
        {
            return _socketClient.SubscribeCandlesAsync(pair, periodInSec, from.GetValueOrDefault(),
                to.GetValueOrDefault(), (int)count.GetValueOrDefault());
        }

        public Task SubscribeTradesAsync(CurrencyPair pair, int maxCount = 100)
        {
            return _socketClient.SubscribeTradesAsync(pair, maxCount);
        }

        public Task UnsubscribeCandlesAsync(CurrencyPair pair)
        {
            return _socketClient.UnsubscribeChannelAsync(pair, "candles");
        }

        public Task UnsubscribeTradesAsync(CurrencyPair pair)
        {
            return _socketClient.UnsubscribeChannelAsync(pair, "trades");
        }

        public void Dispose()
        {
            _socketClient.DisconnectAsync()
                .Wait();
        }

        private void CandleExecuted(IEnumerable<Candle> candles)
        {
            if (CandleSeriesProcessing == null)
                return;

            foreach (var candle in candles)
                CandleSeriesProcessing.Invoke(candle);
        }

        private void TradesExecuted(Trade trade)
        {
            if (trade.Side == "buy")
            {
                NewBuyTrade?.Invoke(trade);
            }
            else if (trade.Side == "sell")
            {
                NewSellTrade?.Invoke(trade);
            }
        }

        public static BitfinexConnector Create()
        {
            var converter = new BitfinexConverter();

            var rest = new BitfinexRestClient(converter);
            var socket = new BitfinexSocketClient(converter);

            return new BitfinexConnector(rest, socket);
        }
    }
}
