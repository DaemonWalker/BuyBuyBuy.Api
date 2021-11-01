using BuyBuyBuy.Api.Contract;
using BuyBuyBuy.Api.Contract.Data;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Service
{
    public class ItemService
    {
        private readonly ICache cache;
        private readonly IStore store;
        public ItemService(ICache cache, IStore store)
        {
            this.cache = cache;
            this.store = store;
        }

        public async Task<bool> BuyOneItem(string itemId)
        {
            var item = await store.GetItemByIdAsync(itemId);
            var idx = await cache.BuyOneItemAsync(itemId);
            if (idx < item.MaxCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
