using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Exchange.Connectors
{
    public class WebSocketSession : IDisposable
    {
        private readonly ClientWebSocket _socket;

        public Task Execution { get; }

        public CancellationTokenSource Cancellation { get; }

        public WebSocketState State => _socket.State;

        private WebSocketSession(ClientWebSocket socket, Task execution, 
            CancellationTokenSource cancellation)
        {
            _socket = socket;

            Execution = execution;
            Cancellation = cancellation;
        }

        public Task SendAsync(byte[] data, WebSocketMessageType messageType)
        {
            return _socket.SendAsync(data, messageType, true, CancellationToken.None);
        }

        public Task SendTextAsync(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);

            return SendAsync(bytes, WebSocketMessageType.Text);
        }

        public Task DisconnectAsync()
        {
            return _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        }

        public static async Task<WebSocketSession> CreateSessionAsync(Uri uri, Action<JsonElement> onMessageRecived)
        {
            var client = new ClientWebSocket();

            await client.ConnectAsync(uri, CancellationToken.None);

            var cancelSource = new CancellationTokenSource();

            var execution = CreateExecutionAsync(client, onMessageRecived, cancelSource);

            return new WebSocketSession(client, execution, cancelSource);
        }

        public void Dispose()
        {
            if (Cancellation.IsCancellationRequested == false)
                Cancellation.Cancel();
        }

        private static async Task CreateExecutionAsync(ClientWebSocket client, 
            Action<JsonElement> onMessageRecived, CancellationTokenSource source)
        {
            using var stream = new MemoryStream();
            try
            {
                while (source.IsCancellationRequested == false && client.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult result = null!;
                    do
                    {
                        var buffer = WebSocket.CreateClientBuffer(2048, 16);
                        result = await client.ReceiveAsync(buffer, source.Token);

                        if (buffer.Array == null)
                            continue;

                        stream.Write(buffer.Array, buffer.Offset, result.Count);
                    }
                    while (result.EndOfMessage == false && source.IsCancellationRequested == false);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Position);
                        var json = JsonSerializer.Deserialize<JsonDocument>(message);

                        if (json == null)
                            continue;

                        onMessageRecived.Invoke(json.RootElement);
                    }

                    stream.Seek(0, SeekOrigin.Begin);
                }
            }
            finally
            {
                await client.CloseAsync(WebSocketCloseStatus.Empty, string.Empty, CancellationToken.None);
            }
        }
    }
}
