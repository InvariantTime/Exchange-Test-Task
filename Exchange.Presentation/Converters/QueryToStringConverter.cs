using Exchange.Domain;
using System.Globalization;
using System.Windows.Data;

namespace Exchange.Presentation.Converters
{
    public class QueryToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Candle candle)
            {
                return $"""
                    [{candle.OpenTime}]: {candle.Pair.First}-{candle.Pair.Second} 
                    low: {candle.LowPrice} high: {candle.HighPrice}
                    open: {candle.OpenPrice} close: {candle.ClosePrice}
                    volume: {candle.TotalVolume}
                """;
            }

            if (value is Trade trade)
            {
                return $"""
                    [{trade.Time}]: {trade.Pair.First}-{trade.Pair.Second} 
                    {trade.Side} for {trade.Price} 
                    amount: {trade.Amount}
                """;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
