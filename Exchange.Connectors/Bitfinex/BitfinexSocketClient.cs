using System.Net.WebSockets;

namespace Exchange.Connectors.Bitfinex
{
    public class BitfinexSocketClient
    {
        private static readonly Uri _uri = new("");

        private readonly ClientWebSocket _socket;

        private BitfinexSocketClient(ClientWebSocket socket)
        {
            _socket = socket;
        }

        public async static Task<BitfinexSocketClient> Connect()
        {
            var socket = new ClientWebSocket();
            await socket.ConnectAsync(_uri, default);

            return new BitfinexSocketClient(socket);
        }
    }
}
