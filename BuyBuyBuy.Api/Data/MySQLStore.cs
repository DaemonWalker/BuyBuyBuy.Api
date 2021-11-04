using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Data
{
    public class MySQLStore : IActivityRepository, IItemRepository
    {
        private readonly IFreeSql fsql;
        public MySQLStore(IFreeSql fsql)
        {
            this.fsql = fsql;
        }
        public async ValueTask<Activity> GetActivityByIdAsync(int Id)
        {
            return await fsql.Select<Activity>().Where(p => p.Id == Id).FirstAsync();
        }

        public async ValueTask<List<Item>> GetActivityItemsAsync(int actId)
        {
            return await fsql.Select<Item>()
                .Where(p => p.ActivityId == actId)
                .Include(p => p.Activity)
                .ToListAsync();
        }

        public async ValueTask<Item> GetItemByIdAsync(string itemId)
        {
            return await fsql.Select<Item>().Where(p=>p.Id==itemId).Include(p=>p.Activity).FirstAsync();
        }
    }
}
