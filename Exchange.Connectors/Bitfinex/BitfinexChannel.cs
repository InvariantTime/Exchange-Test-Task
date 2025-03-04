using Exchange.Domain;

namespace Exchange.Connectors.Bitfinex
{
    public class BitfinexChannel
    {
        public int Id { get; private set; }

        public CurrencyPair Pair { get; }

        public string Type { get; }

        public int Limit { get; }

        public DateTimeOffset From { get; }

        public DateTimeOffset To { get; }

        public BitfinexChannel(CurrencyPair pair, string type, int limit, DateTimeOffset to = default, DateTimeOffset from = default)
        {
            Pair = pair;
            Type = type;
        }

        public void SetId(int id)
        {
            Id = id;
        }
    }
}
