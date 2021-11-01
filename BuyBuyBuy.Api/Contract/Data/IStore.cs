using BuyBuyBuy.Api.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Contract.Data
{
    public interface IStore
    {
        Task<Item> GetItemByIdAsync(string itemId);
        Task<List<Item>> GetAllItems();
        Task<Activity> GetActivityByIdAsync(int Id);
    }
}
