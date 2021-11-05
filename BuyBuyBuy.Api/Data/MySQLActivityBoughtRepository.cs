using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Data
{
    public class MySQLActivityBoughtRepository : IActivityBoughtRepository
    {
        private readonly IFreeSql fsql;
        public MySQLActivityBoughtRepository(IFreeSql fsql)
        {
            this.fsql = fsql;
        }
        public Task AddUserBuyAsync(UserBought history)
        {
            return fsql.Insert(history).ExecuteAffrowsAsync();
        }

        public Task<List<UserBought>> GetActivityUserBought(int actId, string userId)
        {
            return fsql.Select<UserBought>().Where(p => p.ActivityId == actId && p.UserId == userId).ToListAsync();
        }

        public Task<List<string>> GetActivityUsers(int actId)
        {
            return fsql.Select<UserBought>().GroupBy(p => p.UserId).ToListAsync(p => p.Key);
        }
    }
}
