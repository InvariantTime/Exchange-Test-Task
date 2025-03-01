using System.Net.WebSockets;

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

        public Task SendAsync()
        {
            return Task.CompletedTask;
        }

        public Task DisconnectAsync()
        {
            return _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        }

        public static async Task<WebSocketSession> CreateSessionAsync(Uri uri, Action<object> onMessageRecived)
        {
            var client = new ClientWebSocket();

            await client.ConnectAsync(uri, CancellationToken.None);

            var cancelSource = new CancellationTokenSource();

            var execution = CreateExecutionAsync(client, cancelSource);

            return new WebSocketSession(client, execution, cancelSource);
        }

        public void Dispose()
        {
            if (Cancellation.IsCancellationRequested == false)
                Cancellation.Cancel();
        }

        private static async Task CreateExecutionAsync(ClientWebSocket client, CancellationTokenSource source)
        {
            using var stream = new MemoryStream();

            while (source.IsCancellationRequested == false && client.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = null!;

                do
                {
                    var buffer = WebSocket.CreateClientBuffer(2048, 0);
                    result = await client.ReceiveAsync(buffer, source.Token);
                }
                while (result.EndOfMessage == false && source.IsCancellationRequested == false);

                
            }

            await client.CloseAsync(WebSocketCloseStatus.Empty, string.Empty, CancellationToken.None);
        }
    }
}
