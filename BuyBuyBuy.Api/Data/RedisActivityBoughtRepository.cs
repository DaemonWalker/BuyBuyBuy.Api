﻿using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Entity;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Data
{
    public class RedisActivityBoughtRepository : IActivityBoughtRepository
    {
        private readonly IDatabase redis;
        public RedisActivityBoughtRepository(IDatabase redis)
        {
            this.redis = redis;
        }
        private string GetHistoryKey(UserBought history) => $"hash_history_{history.UserId}_{history.ActivityId}";
        private string GetHistoryLogKey(UserBought history) => $"list_history_{history.ActivityId}";
        private string GetActivityUserKey(int actId) => $"set_actusers_{actId}";
        private string BuildHistoryLog(UserBought history) =>
            $"{DateTime.Now:yyyy/MM/dd HH:mm:ss} {history.UserId} buy {history.Quantity} {history.ItemId} in act{history.ActivityId}";


        public async Task AddUserBuyAsync(UserBought history)
        {
            await redis.HashIncrementAsync(GetHistoryKey(history), history.ItemId, history.Quantity);
            await redis.SetAddAsync(GetActivityUserKey(history.ActivityId), history.UserId);
            await redis.ListLeftPushAsync(GetHistoryLogKey(history), BuildHistoryLog(history));
        }

        public async Task<List<string>> GetActivityUsers(int actId)
        {
            var users = await redis.SetMembersAsync(GetActivityUserKey(actId));
            return users.Select(p => p.ToString()).ToList();
        }

        public async Task<List<UserBought>> GetActivityUserBought(int actId, string userId)
        {
            var result = new List<UserBought>();
            var history = await redis.HashGetAllAsync(
                    GetHistoryKey(new UserBought() { ActivityId = actId, UserId = userId }));

            foreach (var item in history)
            {
                var itemId = item.Name.ToString();
                var count = (int)item.Value;
                result.Add(new UserBought() { ActivityId = actId, UserId = userId, ItemId = itemId, Quantity = count });
            }
            return result;
        }
    }
}
