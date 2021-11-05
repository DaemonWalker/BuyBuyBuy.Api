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
    }
}
