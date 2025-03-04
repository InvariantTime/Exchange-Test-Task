namespace Exchange.Connectors.Bitfinex
{
    public static class BitfinexTime
    {
        private static readonly DateTimeOffset _unixOffset = new(1970, 1, 1, 0, 0, 0, default);

        public static string ConvertSecondsPeriod(int period)
        {
            var symbol = period switch
            {
                60 => "1m",
                300 => "5m",
                900 => "15m",
                1800 => "30m",
                3600 => "1h",
                10800 => "3h",
                21600 => "6h",
                43200 => "12h",
                86400 => "1D",
                604800 => "1W",
                1209600 => "14D",
                > 1209600 => "1M",
                _ => throw new NotSupportedException()
            };

            return symbol;
        }

        public static DateTimeOffset ConvertTime(long mts)
        {
            return _unixOffset.AddMilliseconds(mts);
        }
    }
}
