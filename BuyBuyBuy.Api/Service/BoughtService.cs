using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Entity;
using BuyBuyBuy.Api.Model;
using BuyBuyBuy.Api.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Service
{
    public class BoughtService
    {
        private readonly IActivityBoughtRepository activityHistoryRepo;
        private readonly IActivityRepository activityRepo;
        private readonly IActivityItemRepository activityItemRepository;
        public BoughtService(IActivityBoughtRepository activityHistory, IActivityRepository activity,
            IActivityItemRepository activityItemRepository)
        {
            this.activityHistoryRepo = activityHistory;
            this.activityRepo = activity;
            this.activityItemRepository = activityItemRepository;
        }
        public async ValueTask<List<UserBoughtModel>> GetActivityBought(int actId)
        {
            var users = await activityHistoryRepo.GetActivityUsers(actId);
            var result = new List<UserBoughtModel>();
            foreach (var user in users)
            {
                result.AddRange(await GetActivityUserBought(actId, user));
            }
            return result;
        }

        public async ValueTask<List<UserBoughtModel>> GetActivityUserBought(int actId, string userId)
        {
            var boughtHistory = await activityHistoryRepo.GetActivityUserBought(actId, userId);
            var itemList = await activityItemRepository.GetItemsAsync(actId);
            var activity = await activityRepo.GetActivityByIdAsync(actId);
            var result = new List<UserBoughtModel>();

            foreach (var history in boughtHistory)
            {
                var item = new UserBoughtModel();
                item.Item = new ItemModel()
                {
                    Name = history.ItemName,
                    Price = history.Price,
                    Url = history.Url,
                };
                item.Quantity = history.Quantity;
                item.Activity = activity;
                item.Time = history.BoughtTime.ToDisplayString();
                result.Add(item);
            }

            return result;
        }
    }
}
