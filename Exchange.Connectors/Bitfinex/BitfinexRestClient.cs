using Exchange.Domain;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Exchange.Connectors.Bitfinex
{
    public class BitfinexRestClient
    {
        private const string _pairFormat = "t{0}{1}";

        private const string _urlTrades = "https://api-pub.bitfinex.com/v2/trades/";
        private const string _urlCandles = "https://api-pub.bitfinex.com/v2/candles/";

        private readonly HttpClient _http;
        private readonly BitfinexConverter _converter;

        public BitfinexRestClient(BitfinexConverter converter)
        {
            _http = new HttpClient();
            _converter = converter;
        }

        public async Task<IEnumerable<Trade>> GetTradesAsync(CurrencyPair pair, int count)
        {
            var symbol = pair.Format(_pairFormat);
            var json = await _http.GetFromJsonAsync<JsonArray>(_urlTrades + $"{symbol}/hist?limit={count}");

            if (json == null)
                return [];

            var trades = _converter.ConvertTrades(pair, json);

            return trades;
        }

        public async Task<IEnumerable<Candle>> GetCandlesAsync(CurrencyPair pair, int periodInSeconds, DateTimeOffset from, DateTimeOffset to)
        {
            var symbol = pair.Format(_pairFormat);
            var period = _converter.ConvertSecondsPeriod(periodInSeconds);

            var json = await _http.GetFromJsonAsync<JsonArray>(_urlCandles + $"{symbol}:{period}/hist?start={from.Millisecond}&end={to.Millisecond}");
            
            if (json == null)
                return Enumerable.Empty<Candle>();

            var candles = _converter.ConvertCandles(pair, json);

            return candles;
        }
    }
}
