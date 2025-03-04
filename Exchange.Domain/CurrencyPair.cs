namespace Exchange.Domain
{
    public readonly record struct CurrencyPair
    {
        public string First { get; init; }

        public string Second { get; init; }

        public CurrencyPair(string first, string second)
        {
            First = first;
            Second = second;
        }

        public string Format(string pattern)
        {
            return string.Format(pattern, First, Second);
        }
    }
}
