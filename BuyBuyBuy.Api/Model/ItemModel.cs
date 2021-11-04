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
        public int TotalInventory { get; set; }
        public int UserLimit { get; set; }
        //public static implicit operator ItemModel(Item item)
        //{
        //    if (item == default)
        //    {
        //        return default;
        //    }
        //    return new ItemModel()
        //    {
        //        Name = item.Name,
        //        ItemId = item.Id,
        //        TotalInventory = item.TotalInventory,
        //        Price = item.Price,
        //        Url = item.Url,
        //    };
        //}
        public static implicit operator ItemModel(Item item)
        {
            if (item == default)
            {
                return default;
            }
            return new ItemModel()
            {
                ItemId = item.Id,
                TotalInventory = item.TotalInventory,
                Price = item.Price,
                Name = item.Name,
                Url = item.Url,
                UserLimit = item.UserLimit,
            };
        }
    }
}
