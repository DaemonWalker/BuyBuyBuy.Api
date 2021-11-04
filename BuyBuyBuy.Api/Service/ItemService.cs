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
        private readonly IItemRepository itemRepository;

        private readonly IActivityBoughtRepository activityHistory;

        public ItemService(ICache cache, IItemRepository itemRepository, IActivityBoughtRepository activityHistory)
        {
            this.cache = cache;
            this.activityHistory = activityHistory;
            this.itemRepository = itemRepository;
        }

        public async Task<BuyItemResult> BuyOneItem(BuyItemModel buy)
        {
            BuyItemResult result = BuyItemResult.OK;
            long buyCount = await cache.AddUserBuyAsync(buy);
            var item = await itemRepository.GetItemByIdAsync(buy.ItemId);
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
            return (await itemRepository.GetActivityItemsAsync(actId)).Cast<ItemModel>().ToList();
        }
    }

}
