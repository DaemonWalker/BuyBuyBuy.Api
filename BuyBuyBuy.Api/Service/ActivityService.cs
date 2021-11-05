using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Entity;
using BuyBuyBuy.Api.Model;
using BuyBuyBuy.Api.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Service
{
    public class ActivityService
    {
        private readonly IActivityRepository activityRepository;
        private readonly IItemRepository itemRepository;
        private readonly IActivityItemRepository activityItemRepository;
        private readonly CurrentTimeAccessor currentTimeAccessor;
        private readonly ICache cache;
        public ActivityService(IActivityRepository store, IItemRepository itemRepository, ICache cache,
            IActivityItemRepository activityItemRepository, CurrentTimeAccessor currentTimeAccessor)
        {
            this.activityRepository = store;
            this.itemRepository = itemRepository;
            this.currentTimeAccessor = currentTimeAccessor;
            this.cache = cache;
            this.activityItemRepository = activityItemRepository;
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
        public async Task<ActivityModel> GetNextActivityAsync()
        {
            bool isStart = true;
            var act = await activityRepository.GetLiveActivityAsync();
            if (act == default)
            {
                isStart = false;
                act = await activityRepository.GetNextHourActivityAsync();
                if (act == default)
                {
                    act = new Activity() { Id = -1 };
                }
            }
            var result = (ActivityModel)act;
            result.IsStart = isStart;
            return result;
        }

        public async ValueTask InitInventory(int actId)
        {
            var list = await this.activityItemRepository.GetItemsAsync(actId);
            await cache.SetActivityInventory(list);
        }

        public async Task<List<ActivityModel>> GetAllActivities()
        {
            var list = await this.activityRepository.GetAll();
            return list.Select(p => (ActivityModel)p).ToList();
        }
    }
}
