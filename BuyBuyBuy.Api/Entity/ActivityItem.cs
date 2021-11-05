using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Entity
{
    [Table(Name = "activity_item")]
    public class ActivityItem
    {
        [Column(IsPrimary = true, IsIdentity = true, Name = "id")]
        public int Id { get; set; }

        [Column(Name = "activity_id")]
        public int ActivityId { get; set; }

        [Column(Name = "item_id")]
        public string ItemId { get; set; }

        [Column(Name = "price")]
        public float Price { get; set; }

        [Column(Name = "inventory")]
        public int Inventory { get; set; }

        [Column(Name = "user_limit")]
        public int UserLimit { get; set; }

        [Navigate(nameof(ItemId))]
        public Item Item { get; set; }

        [Navigate(nameof(ActivityId))]
        public Activity Activity { get; set; }
    }
}
