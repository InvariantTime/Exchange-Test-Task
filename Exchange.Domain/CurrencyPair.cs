namespace Exchange.Domain
{
    public readonly record struct CurrencyPair(Currency First, Currency Second, decimal Ratio);
}
