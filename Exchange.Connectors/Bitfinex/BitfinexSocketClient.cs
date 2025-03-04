using Exchange.Domain;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Exchange.Connectors.Bitfinex
{
    public class BitfinexSocketClient
    {
        private const string _pairFormat = "t{0}{1}";
        private const string _tradeChannel = "trades";
        private const string _candleChannel = "candles";

        private static readonly Uri _uri = new("wss://api-pub.bitfinex.com/ws/2");

        private readonly List<BitfinexChannel> _channels;
        private readonly BitfinexConverter _converter;

        private WebSocketSession? _session;

        public event Action<Trade>? TradeExecuted;
        public event Action<IEnumerable<Candle>>? CandleExecuted;

        public bool IsConnected => _session != null && _session.State == WebSocketState.Open;

        public BitfinexSocketClient(BitfinexConverter converter)
        {
            _channels = new();
            _converter = converter;
        }

        public async Task ConnectAsync()
        {
            if (_session != null && _session.State == WebSocketState.Open)
                throw new Exception("Client is already connected");

            _session?.Dispose();
            _channels.Clear();

            _session = await WebSocketSession.CreateSessionAsync(_uri, OnMessageRecived);
        }

        public async Task SubscribeTradesAsync(CurrencyPair pair, int limit)
        {
            if (IsConnected == false)
                await ConnectAsync();

            var symbol = pair.Format(_pairFormat);

            string query = $$"""
                {
                    "event": "subscribe",
                    "channel": "trades",
                    "symbol": "{{symbol}}",
                    "limit": "{{limit}}"
                }
                """;

            await _session!.SendTextAsync(query);
            _channels.Add(new BitfinexChannel(pair, _tradeChannel, limit));
        }

        public async Task SubscribeCandlesAsync(CurrencyPair pair, 
            int frameInSeconds, DateTimeOffset from, DateTimeOffset to, int limit)
        {
            if (IsConnected == false)
                await ConnectAsync();

            var frame = BitfinexTime.ConvertSecondsPeriod(frameInSeconds);
            var symbol = pair.Format(_pairFormat);

            string query = $$"""
                {
                    "event": "subscribe",
                    "channel": "candles",
                    "key": "trade:{{frame}}:{{symbol}}"
                }
            """;

            await _session!.SendTextAsync(query);
            _channels.Add(new BitfinexChannel(pair, _candleChannel, limit, to, from));
        }

        public async Task UnsubscribeChannelAsync(CurrencyPair pair, string channel)
        {
            if (IsConnected == false)
                return;

            var data = _channels.FirstOrDefault(x => x.Pair == pair && x.Type == channel);

            if (data == null)
                return;

            string query = $$"""
                    {
                        "event": "unsubscribe",
                        "chanId": {{data.Id}}
                    }
                """;

            await _session!.SendTextAsync(query);
            _channels.Remove(data);
        }

        public Task DisconnectAsync()
        {
            return _session?.DisconnectAsync() ?? Task.CompletedTask;
        }

        private void OnMessageRecived(JsonElement json)
        {
            if (json.ValueKind == JsonValueKind.Object)
            {
                if (json.TryGetProperty("event", out var @event) == true)
                {
                    if (@event.GetString() == "subscribed")
                        HandleSubscribeEvent(json);
                }
            }
            else if (json.ValueKind == JsonValueKind.Array)
            {
                var id = json[0].GetInt32();

                var channel = _channels.FirstOrDefault(x => x.Id == id);

                if (channel == null)
                    return;

                if (channel.Type == _tradeChannel && json[1].ValueKind == JsonValueKind.String && json[1].GetString() == "te")
                {
                    HandleTrades(json, channel);
                }
                else if (channel.Type == _candleChannel && json[1].ValueKind == JsonValueKind.Array)
                {
                    HandleCandles(json, channel);
                }
            }
        }

        private void HandleSubscribeEvent(JsonElement element)
        {
            var type = element.GetProperty("channel").GetString();
            var id = element.GetProperty("chanId").GetInt32();
            var symbol = string.Empty;

            if (type == _tradeChannel)
            {
                symbol = element.GetProperty("symbol").GetString();
            }
            else if (type == _candleChannel)
            {
                var key = element.GetProperty("key").GetString() ?? string.Empty;
                var keys = key.Split(':');

                symbol = keys.Length > 2 ? keys[2] : string.Empty;
            }

            if (symbol == string.Empty)
                return;

            var channel = _channels.FirstOrDefault(x => x.Type == type && x.Pair.Format(_pairFormat) == symbol);

            if (channel == null)
                return;

            channel.SetId(id);
        }

        private void HandleTrades(JsonElement element, BitfinexChannel channel)
        {
            var trade = _converter.ConvertTrade(channel.Pair, element[2]);
            TradeExecuted?.Invoke(trade);
        }

        private void HandleCandles(JsonElement element, BitfinexChannel channel)
        {
            var candles = _converter.ConvertCandles(channel.Pair, JsonArray.Create(element[1])!);

            if (channel.Limit > 0)
                candles = candles.Take(channel.Limit);

            CandleExecuted?.Invoke(candles);
        }
    }
}