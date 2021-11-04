using BuyBuyBuy.Api.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Contract.Data
{
    public interface IItemRepository
    {
        ValueTask<Item> GetItemByIdAsync(string itemId);
        ValueTask<List<Item>> GetActivityItemsAsync(int actId);
    }
}
