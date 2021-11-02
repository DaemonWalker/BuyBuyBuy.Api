namespace BuyBuyBuy.Api.Entity
{
    public class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public static Item Create(string itemId, string name, string url)
        {
            return new Item() { Id = itemId, Name = name, Url = url };
        }
    }
}
