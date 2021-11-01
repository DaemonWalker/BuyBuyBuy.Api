using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Contract
{
    public interface ICache
    {
        Task<long> BuyOneItemAsync(string itemId);
        long BuyOneItem(string itemId);
    }
}
