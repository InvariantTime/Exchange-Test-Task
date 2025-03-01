using Exchange.Domain;
using System.Net.WebSockets;

namespace Exchange.Connectors.Bitfinex
{
    public class BitfinexSocketClient
    {
        private static readonly Uri _uri = new("wss://api-pub.bitfinex.com/ws/2");

        private WebSocketSession? _session;

        public event Action<Trade>? TradeExecuted;
        public event Action<Candle>? CandleExecuted;

        public async Task ConnectAsync()
        {
            if (_session != null && _session.State == WebSocketState.Open)
                throw new Exception("Client is already connected");

            _session?.Dispose();
            _session = await WebSocketSession.CreateSessionAsync(_uri, OnMessageRecived);
        }

        public Task SubscribeChannelAsync(BitfinexSubscribeInfo info)
        {
            return Task.CompletedTask;
        }

        public Task UnsubscribeChannelAsync(BitfinexUnsubscribeInfo info)
        {
            return Task.CompletedTask;
        }

        public Task DisconnectAsync()
        {
            return _session?.DisconnectAsync() ?? Task.CompletedTask;
        }

        private void OnMessageRecived(object o)
        {

        }
    }
}