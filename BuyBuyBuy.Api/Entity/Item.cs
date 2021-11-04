using FreeSql.DataAnnotations;

namespace BuyBuyBuy.Api.Entity
{
    [Table(Name = "item")]
    public class Item
    {
        [Column(IsIdentity = true, Name = "id")]
        public string Id { get; set; }

        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "url")]
        public string Url { get; set; }

        [Column(Name = "act_id")]
        public int ActivityId { get; set; }

        [Column(Name = "price")]
        public double Price { get; set; }

        [Column(Name = "total_inventory")]
        public int TotalInventory { get; set; }

        [Column(Name = "user_limit")]
        public int UserLimit { get; set; }

        [Navigate(nameof(ActivityId))]
        public Activity Activity { get; set; }

        public static Item Create(string itemId, string name, string url)
        {
            return new Item() { Id = itemId, Name = name, Url = url };
        }
    }
}
