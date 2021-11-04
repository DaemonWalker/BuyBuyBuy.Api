using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Model;
using BuyBuyBuy.Api.Tools;
using System;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Service
{
    public class ActivityService
    {
        private readonly IActivityRepository activityRepository;
        private readonly IItemRepository itemRepository;
        private readonly CurrentTimeAccessor currentTimeAccessor;
        private readonly ICache cache;
        public ActivityService(IActivityRepository store, IItemRepository itemRepository, ICache cache,
            CurrentTimeAccessor currentTimeAccessor)
        {
            this.activityRepository = store;
            this.itemRepository = itemRepository;
            this.currentTimeAccessor = currentTimeAccessor;
            this.cache = cache;
        }
        public async ValueTask<bool> CheckActivityIsActive(int actId)
        {
            var activity = await activityRepository.GetActivityByIdAsync(actId);
            if (activity == default)
            {
                return false;
            }
            var currentTime = currentTimeAccessor.Access();
            return activity.Start < currentTime && currentTime < activity.End;
        }
        public async ValueTask<ActivityModel> GetActivity(int actId)
        {
            return await activityRepository.GetActivityByIdAsync(actId);
        }
        public async ValueTask InitInventory(int actId)
        {
            var list = await this.itemRepository.GetActivityItemsAsync(actId);
            await cache.SetActivityInventory(list);
        }
    }
}
