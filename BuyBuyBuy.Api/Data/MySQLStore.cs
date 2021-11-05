using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Entity;
using BuyBuyBuy.Api.Tools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Data
{
    public class MysqlStore : IActivityRepository, IItemRepository, IActivityItemRepository, IUserRepository
    {
        private readonly IFreeSql fsql;
        private readonly CurrentTimeAccessor currentTimeAccessor;
        public MysqlStore(IFreeSql fsql, CurrentTimeAccessor currentTimeAccessor)
        {
            this.fsql = fsql;
            this.currentTimeAccessor = currentTimeAccessor;
        }

        public async Task<User> CreateOrUpdateUser(User user)
        {
            await fsql.InsertOrUpdate<User>().SetSource(user).ExecuteAffrowsAsync();
            return await fsql.Select<User>().Where(p => p.Id == user.Id).FirstAsync();
        }

        public Task<Activity> GetActivityByIdAsync(int Id)
        {
            return fsql.Select<Activity>().Where(p => p.Id == Id).FirstAsync();
        }

        public Task<List<Activity>> GetAll()
        {
            return fsql.Select<Activity>().ToListAsync();
        }

        public Task<User> GetByIdAsync(string id)
        {
            return fsql.Select<User>().Where(p => p.Id == id).ToOneAsync();
        }

        public Task<ActivityItem> GetItemByIdAsync(int actId, string itemId)
        {
            return fsql.Select<ActivityItem>()
                .Where(p => p.ActivityId == actId && p.ItemId == itemId)
                .Include(p => p.Item)
                .FirstAsync();
        }

        public Task<List<ActivityItem>> GetItemsAsync(int actId)
        {
            return fsql.Select<ActivityItem>()
                .Where(p => p.ActivityId == actId)
                .Include(p => p.Item)
                .ToListAsync();
        }

        public Task<Activity> GetLiveActivityAsync()
        {
            var dt = this.currentTimeAccessor.Access();
            return GetActivityByTime(dt);
        }

        public Task<Activity> GetNextHourActivityAsync()
        {
            var dt = this.currentTimeAccessor.Access().AddHours(1);
            return GetActivityByTime(dt);
        }

        private Task<Activity> GetActivityByTime(DateTime dt)
        {
            return fsql.Select<Activity>()
                .Where(p => p.Start < dt && dt < p.End)
                .ToOneAsync();
        }

    }
}
