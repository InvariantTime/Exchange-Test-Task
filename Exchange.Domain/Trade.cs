﻿
namespace Exchange.Domain
{
    public class Trade
    {
        /// <summary>
        /// Валютная пара
        /// </summary>
        public CurrencyPair Pair { get; set; }

        /// <summary>
        /// Цена трейда
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Объем трейда
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Направление (buy/sell)
        /// </summary>
        public string Side { get; set; } = string.Empty;

        /// <summary>
        /// Время трейда
        /// </summary>
        public DateTimeOffset Time { get; set; }


        /// <summary>
        /// Id трейда
        /// </summary>
        public string Id { get; set; } = string.Empty;

    }
}
