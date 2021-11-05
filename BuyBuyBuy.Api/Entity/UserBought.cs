using BuyBuyBuy.Api.Model;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Entity
{
    [Table(Name = "user_bought")]
    public class UserBought
    {
        [Column(Name = "id", IsPrimary = true, IsIdentity = true)]
        public int Id { get; set; }

        [Column(Name = "activity_id")]
        public int ActivityId { get; set; }

        [Column(Name = "item_id")]
        public string ItemId { get; set; }

        [Column(Name = "item_url")]
        public string Url { get; set; }

        [Column(Name = "item_name")]
        public string ItemName { get; set; }

        [Column(Name = "quantity")]
        public long Quantity { get; set; }

        [Column(Name = "price")]
        public float Price { get; set; }

        [Column(Name = "bought_time", IsNullable = false)]
        public DateTime BoughtTime { get; set; }

        [Column(Name = "user_id")]
        public string UserId { get; set; }

        public static UserBought Create(BuyItemModel buy, ActivityItem item, DateTime now)
        {
            return new UserBought()
            {
                ActivityId = buy.ActivityId,
                ItemId = buy.ItemId,
                Quantity = buy.Quantity,
                UserId = buy.UserId,
                Price = item.Price,
                Url = item.Item.Url,
                ItemName = item.Item.Name,
                BoughtTime = now
            };
        }
    }
}
