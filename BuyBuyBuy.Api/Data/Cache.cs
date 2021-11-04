using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Entity;
using BuyBuyBuy.Api.Model;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Data
{
    public class Cache : ICache
    {
        private readonly IDatabase redis;
        public Cache(IDatabase redis)
        {
            this.redis = redis;
        }

        public Task<long> AddUserBuyAsync(BuyItemModel buy)
        {
            return redis.HashIncrementAsync(BuildUserBoughtKey(buy), buy.UserId, buy.Quantity);
        }

        public Task<long> BuyItemAsync(BuyItemModel buy)
        {
            return redis.HashDecrementAsync(BuildActivityInventoryKey(buy.ActivityId), buy.ItemId, buy.Quantity);
        }

        public Task SetActivityInventory(List<Item> items)
        {
            var activityId = items.First().ActivityId;
            var entries = items.Select(p => new HashEntry(p.Id, p.TotalInventory)).ToArray();
            return redis.HashSetAsync(BuildActivityInventoryKey(activityId), entries);
        }

        private string BuildUserBoughtKey(BuyItemModel buy) => $"hash_bought_{buy.UserId}_{buy.ActivityId}";
        private string BuildActivityInventoryKey(int activityId) => $"hash_inventory_{activityId}";
    }
}
