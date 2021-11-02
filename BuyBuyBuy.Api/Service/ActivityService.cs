using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Model;
using BuyBuyBuy.Api.Tools;
using System;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Service
{
    public class ActivityService
    {
        private readonly IStore store;
        private readonly IActionItemRepository actionItem;
        private readonly CurrentTimeAccessor currentTimeAccessor;
        private readonly ICache cache;
        public ActivityService(IStore store, IActionItemRepository actionItem, ICache cache,
            CurrentTimeAccessor currentTimeAccessor)
        {
            this.store = store;
            this.actionItem = actionItem;
            this.currentTimeAccessor = currentTimeAccessor;
            this.cache = cache;
        }
        public async ValueTask<bool> CheckActivityIsActive(int actId)
        {
            var activity = await store.GetActivityByIdAsync(actId);
            if (activity == default)
            {
                return false;
            }
            var currentTime = currentTimeAccessor.Access();
            return activity.Start < currentTime && currentTime < activity.End;
        }
        public async ValueTask<ActivityModel> GetActivity(int actId)
        {
            return await store.GetActivityByIdAsync(actId);
        }
        public async ValueTask InitInventory(int actId)
        {
            var list = await actionItem.GetActivityItemsAsync(actId);
            await cache.SetActivityInventory(list);
        }
    }
}
