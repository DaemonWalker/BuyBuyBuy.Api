using BuyBuyBuy.Api.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Model
{
    public class ItemModel
    {
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public double Price { get; set; }
        public int MaxCount { get; set; }
        public static implicit operator ItemModel(ActivityItem item)
        {
            if (item == default)
            {
                return default;
            }
            return new ItemModel()
            {
                Name = item.Item.Name,
                ItemId = item.ItemId,
                MaxCount = item.TotalInventory,
                Price = item.Price,
                Url = item.Item.Url,
            };
        }
    }
}
