using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Entity
{
    public class ActivityItem
    {
        public string ItemId { get; set; }
        public int ActivityId { get; set; }
        public double Price { get; set; }
        public int TotalInventory { get; set; }
        public int UserLimit { get; set; }
        public Item Item { get; set; }
    }
}
