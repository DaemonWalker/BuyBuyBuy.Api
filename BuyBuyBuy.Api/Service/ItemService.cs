using BuyBuyBuy.Api.Constance;
using BuyBuyBuy.Api.Contract;
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
    public class ItemService
    {
        private readonly ICache cache;
        private readonly IActivityItemRepository activityItemRepository;
        private readonly IActivityBoughtRepository activityHistory;
        private readonly CurrentTimeAccessor currentTimeAccessor;

        public ItemService(ICache cache, IActivityItemRepository activityItemRepository,
            IActivityBoughtRepository activityHistory, CurrentTimeAccessor currentTimeAccessor)
        {
            this.cache = cache;
            this.activityHistory = activityHistory;
            this.activityItemRepository = activityItemRepository;
            this.currentTimeAccessor = currentTimeAccessor;
        }

        public async Task<BuyItemResult> BuyOneItem(BuyItemModel buy)
        {
            BuyItemResult result = BuyItemResult.OK;
            long buyCount = await cache.AddUserBuyAsync(buy);
            var item = await activityItemRepository.GetItemByIdAsync(buy.ActivityId, buy.ItemId);
            if (buyCount > item.UserLimit)
            {
                if (buyCount - buy.Quantity >= item.UserLimit)
                {
                    return BuyItemResult.Buyed;
                }
                else
                {
                    result = BuyItemResult.OverLimit;
                    buy.Quantity = item.UserLimit - (buyCount - buy.Quantity);
                }
            }
            long inventory = await cache.BuyItemAsync(buy);
            if (inventory < 0)
            {
                if (inventory + buy.Quantity <= 0)
                {
                    return BuyItemResult.SoldOut;
                }
                else
                {
                    buy.Quantity = inventory + buy.Quantity;
                    result = BuyItemResult.NotEnough;
                }
            }

            await activityHistory.AddUserBuyAsync(UserBought.Create(buy, item, currentTimeAccessor.Access()));
            return result;
        }

        public async Task<List<ItemModel>> GetItemsByActivity(int actId)
        {
            return (await activityItemRepository.GetItemsAsync(actId)).Select(p => (ItemModel)p).ToList();
        }
    }

}
