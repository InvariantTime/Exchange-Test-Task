using Exchange.Presentation.Models;

namespace Exchange.Presentation.Services
{
    public interface ICurrencyConverter
    {
        Task<decimal> ConvertToAsync(string currency, Wallet wallet);
    }
}