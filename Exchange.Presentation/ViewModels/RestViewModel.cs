using Exchange.Connectors;
using Exchange.Domain;
using Exchange.Presentation.Commands;
using Exchange.Presentation.Models;
using System.Collections.ObjectModel;

namespace Exchange.Presentation.ViewModels
{
    public class RestViewModel
    {
        private readonly ITestConnector _connector;
        private readonly Lazy<IAsyncCommand> _sendCommand;

        public QueryTypes Type { get; set; } = QueryTypes.Trades;

        public QueryModel Query { get; }

        public IAsyncCommand SendCommand => _sendCommand.Value;

        public ObservableCollection<object> Result { get; }

        public RestViewModel(ITestConnector connector)
        {
            Query = new();
            Result = new();

            _connector = connector;
            _sendCommand = new Lazy<IAsyncCommand>(() => new AsyncCommand(SendQueryAsync));
        }

        private async Task SendQueryAsync(CancellationToken cancellation)
        {
            Result.Clear();
            var pair = new CurrencyPair(Query.FirstCurrency, Query.SecondCurrency);

            if (Type == QueryTypes.Trades)
            {
                var trades = await _connector.GetNewTradesAsync(pair, Query.Limit);
                
                foreach (var item in trades)
                    Result.Add(item);
            }
            else if (Type == QueryTypes.Candles)
            {
                var from = DateTimeOffset.FromUnixTimeMilliseconds(Query.From);
                var to = DateTimeOffset.FromUnixTimeMilliseconds(Query.To);
                var candles = await _connector.GetCandleSeriesAsync(pair, Query.Frame, from, to, Query.Limit);

                foreach (var item in candles)
                    Result.Add(item);
            }
        }
    }
}
