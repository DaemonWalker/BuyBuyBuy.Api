using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Entity;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Data
{
    public class RedisActivityHistory : IActivityHistory
    {
        private readonly IDatabase redis;
        public RedisActivityHistory(IDatabase redis)
        {
            this.redis = redis;
        }

        public async Task AddUserBuyAsync(UserBuyHistory history)
        {
            await redis.HashIncrementAsync(GetHistoryKey(history), history.ItemId, history.Quantity);
            await redis.ListLeftPushAsync(GetHistoryLogKey(history), BuildHistoryLog(history));
        }

        private string GetHistoryKey(UserBuyHistory history) => $"hash_history_{history.UserId}_{history.ActivityId}";
        private string GetHistoryLogKey(UserBuyHistory history) => $"list_history_{history.ActivityId}";
        private string BuildHistoryLog(UserBuyHistory history) =>
            $"{DateTime.Now:yyyy/MM/dd HH:mm:ss} {history.UserId} buy {history.Quantity} {history.ItemId} in act{history.ActivityId}";
    }
}
