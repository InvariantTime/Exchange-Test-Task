namespace Exchange.Domain
{
    public readonly record struct CurrencyPair
    {
        public string First { get; init; }

        public string Second { get; init; }

        public string Format(string pattern)
        {
            return string.Format(pattern, First, Second);
        }
    }
}
