using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Entity;
using BuyBuyBuy.Api.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Data
{
    public class StaticStore : IStore, IActionItemRepository
    {
        private readonly static IReadOnlyList<Item> items = new List<Item>()
        {
            Item.Create("cherry-keyboard","樱桃机械键盘","http://192.168.12.2/s/PC9dRNt4n2QwZ2n/preview"),
        };

        private readonly static IReadOnlyList<Activity> activities = new List<Activity>()
        {
            new Activity(){ Name="测试秒杀", Id=1,Start=DateTime.MinValue, End=DateTime.MaxValue }
        };
        private readonly static IReadOnlyList<ActivityItem> activityItems = new List<ActivityItem>()
        {
            new ActivityItem(){ ActivityId=1, ItemId="cherry-keyboard", TotalInventory=5, Price=20, Item=items.First(),UserLimit=3 },
        };

        public Task<Item> GetItemByIdAsync(string itemId)
        {
            return Task.FromResult(items.FirstOrDefault(p => p.Id == itemId));
        }

        public Task<List<Item>> GetAllItems()
        {
            return Task.FromResult(new List<Item>(items));
        }

        public Task<Activity> GetActivityByIdAsync(int Id)
        {
            return Task.FromResult(activities.FirstOrDefault(p => p.Id == Id));
        }

        public Task<List<ActivityItem>> GetActivityItemsAsync(int activityId)
        {
            return Task.FromResult(activityItems.Where(p => p.ActivityId == activityId).ToList());
        }

        public Task<ActivityItem> GetOneItemInActivityAsync(int activityId, string itemId)
        {
            return Task.FromResult(activityItems.FirstOrDefault(p => p.ActivityId == activityId && p.ItemId == itemId));
        }
    }
}
