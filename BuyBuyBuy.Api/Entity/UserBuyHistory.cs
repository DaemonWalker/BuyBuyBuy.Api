using BuyBuyBuy.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Entity
{
    public class UserBuyHistory
    {
        public int ActivityId { get; set; }
        public string ItemId { get; set; }
        public long Quantity { get; set; }
        public string UserId { get; set; }
        public static UserBuyHistory Create(BuyItemModel buy)
        {
            return new UserBuyHistory()
            {
                ActivityId = buy.ActivityId,
                ItemId = buy.ItemId,
                UserId = buy.UserId,
                Quantity = buy.Quantity,
            };
        }
    }
}
