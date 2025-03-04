using Exchange.Connectors;
using Exchange.Domain;
using Exchange.Presentation.Commands;
using Exchange.Presentation.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Exchange.Presentation.ViewModels
{
    public class SocketViewModel
    {
        private readonly ITestConnector _connector;
        private readonly Lazy<ICommand> _subscribeCommand;
        private readonly Lazy<ICommand> _unsubscribeCommand;
        private readonly Lazy<ICommand> _clearCommand;

        public QueryTypes Type { get; set; } = QueryTypes.Trades;

        public QueryModel Query { get; }

        public ObservableCollection<object> Result { get; }

        public ICommand SubscribeCommand => _subscribeCommand.Value;

        public ICommand UnsubscribeCommand => _unsubscribeCommand.Value;

        public ICommand ClearCommand => _clearCommand.Value;

        public SocketViewModel(ITestConnector connector)
        {
            Query = new();
            Result = new();

            _connector = connector;
            _connector.NewBuyTrade += OnNewTrade;
            _connector.NewSellTrade += OnNewTrade;
            _connector.CandleSeriesProcessing += OnNewCandle;

            _subscribeCommand = new Lazy<ICommand>(() => new Command(Subscribe));
            _unsubscribeCommand = new Lazy<ICommand>(() => new Command(Unsubscribe));
            _clearCommand = new Lazy<ICommand>(() => new Command(Clear));
        }

        private void Subscribe()
        {
            var pair = new CurrencyPair(Query.FirstCurrency, Query.SecondCurrency);

            if (Type == QueryTypes.Trades)
            {
                _connector.SubscribeTradesAsync(pair, Query.Limit);
            }
            else if (Type == QueryTypes.Candles)
            {
                var from = DateTimeOffset.FromUnixTimeMilliseconds(Query.From);
                var to = DateTimeOffset.FromUnixTimeMilliseconds(Query.To);
                _connector.SubscribeCandlesAsync(pair, Query.Frame, from, to, Query.Limit);
            }
        }

        private void Unsubscribe()
        {
            var pair = new CurrencyPair(Query.FirstCurrency, Query.SecondCurrency);

            if (Type == QueryTypes.Trades)
            {
                _connector.UnsubscribeTradesAsync(pair);
            }
            else if (Type == QueryTypes.Candles)
            {
                _connector.UnsubscribeCandlesAsync(pair);
            }
        }

        private void Clear()
        {
            Result.Clear();
        }

        private void OnNewTrade(Trade trade)
        {
            Result.Add(trade);
        }

        private void OnNewCandle(Candle candle)
        {
            Result.Add(candle);
        }
    }
}
