using BuyBuyBuy.Api.Contract;
using StackExchange.Redis;
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
        public long BuyOneItem(string itemId)
        {
            return redis.StringIncrement(itemId);
        }

        public Task<long> BuyOneItemAsync(string itemId)
        {
            return redis.StringIncrementAsync(itemId);
        }
    }
}
