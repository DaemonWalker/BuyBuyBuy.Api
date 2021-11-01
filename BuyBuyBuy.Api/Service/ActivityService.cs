using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Tools;
using System;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Service
{
    public class ActivityService
    {
        private readonly IStore store;
        private readonly CurrentTimeAccessor currentTimeAccessor;
        public ActivityService(IStore store, CurrentTimeAccessor currentTimeAccessor)
        {
            this.store = store;
            this.currentTimeAccessor = currentTimeAccessor;
        }
        public async Task<bool> CheckActivityIsActive(int activityId)
        {
            var activity = await store.GetActivityByIdAsync(activityId);
            var currentTime = currentTimeAccessor.Access();
            return activity.Start < currentTime && currentTime < activity.End;
        }
    }
}
