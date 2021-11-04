using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Entity;
using BuyBuyBuy.Api.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Service
{
    public class BoughtService
    {
        private readonly IActivityBoughtRepository activityHistoryRepo;
        private readonly IActivityRepository activityRepo;
        private readonly IItemRepository itemRepository;
        public BoughtService(IActivityBoughtRepository activityHistory, IActivityRepository activity,
            IItemRepository itemRepository)
        {
            this.activityHistoryRepo = activityHistory;
            this.activityRepo = activity;
            this.itemRepository = itemRepository;
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
            var itemList = await itemRepository.GetActivityItemsAsync(actId);
            var activity = await activityRepo.GetActivityByIdAsync(actId);
            var result = new List<UserBoughtModel>();

            foreach (var history in boughtHistory)
            {
                var itemEntity = itemList.First(p => p.Id == history.ItemId);
                var item = new UserBoughtModel();
                item.Item = itemEntity;
                item.Quantity = history.Quantity;
                item.Activity = activity;
                result.Add(item);
            }

            return result;
        }
    }
}
