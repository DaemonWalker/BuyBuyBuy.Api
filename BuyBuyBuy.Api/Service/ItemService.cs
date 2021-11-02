using BuyBuyBuy.Api.Constance;
using BuyBuyBuy.Api.Contract;
using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Entity;
using BuyBuyBuy.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Service
{
    public class ItemService
    {
        private readonly ICache cache;

        private readonly IActionItemRepository actionItem;

        private readonly IActivityHistory activityHistory;

        public ItemService(ICache cache, IActionItemRepository actionItem, IActivityHistory activityHistory)
        {
            this.cache = cache;
            this.activityHistory = activityHistory;
            this.actionItem = actionItem;
        }

        public async Task<BuyItemResult> BuyOneItem(BuyItemModel buy)
        {
            BuyItemResult result = BuyItemResult.OK;
            long buyCount = await cache.AddUserBuyAsync(buy);
            ActivityItem item = await actionItem.GetOneItemInActivityAsync(buy.ActivityId, buy.ItemId);
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

            await activityHistory.AddUserBuyAsync(UserBuyHistory.Create(buy));
            return result;
        }

        public async Task<List<ItemModel>> GetItemsByActivity(int actId)
        {
            return (await actionItem.GetActivityItemsAsync(actId)).Cast<ItemModel>().ToList();
        }
    }

}
