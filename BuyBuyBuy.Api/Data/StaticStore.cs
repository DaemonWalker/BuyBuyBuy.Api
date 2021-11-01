using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Entity;
using BuyBuyBuy.Api.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Data
{
    public class StaticStore : IStore
    {
        private readonly static IReadOnlyList<Item> items = new List<Item>()
        {

        };

        private readonly static IReadOnlyList<Activity> activities = new List<Activity>()
        {

        };

        public Task<Item> GetItemByIdAsync(string itemId)
        {
            return Task.FromResult(items.FirstOrDefault(p => p.ItemId == itemId));
        }

        public Task<List<Item>> GetAllItems()
        {
            return Task.FromResult(new List<Item>(items));
        }

        public Task<Activity> GetActivityByIdAsync(int Id)
        {
            return Task.FromResult(activities.FirstOrDefault(p => p.Id == Id));
        }
    }
}
