using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Entity;
using BuyBuyBuy.Api.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Data
{
    public class StaticStore : IActivityRepository, IItemRepository
    {
        private readonly static IReadOnlyList<Item> items = new List<Item>()
        {
            Item.Create("cherry-keyboard","樱桃机械键盘","http://192.168.12.2/s/PC9dRNt4n2QwZ2n/preview"),
        };

        private readonly static IReadOnlyList<Activity> activities = new List<Activity>()
        {
            new Activity(){ Name="测试秒杀", Id=1,Start=DateTime.MinValue, End=DateTime.MaxValue }
        };

        public ValueTask<Item> GetItemByIdAsync(string itemId)
        {
            return ValueTask.FromResult(items.FirstOrDefault(p => p.Id == itemId));
        }

        public ValueTask<List<Item>> GetAllItems()
        {
            return ValueTask.FromResult(new List<Item>(items));
        }

        public ValueTask<Activity> GetActivityByIdAsync(int Id)
        {
            return ValueTask.FromResult(activities.FirstOrDefault(p => p.Id == Id));
        }

        public ValueTask<List<Item>> GetActivityItemsAsync(int actId)
        {
            throw new NotImplementedException();
        }
    }
}
