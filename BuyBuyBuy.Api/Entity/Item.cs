namespace BuyBuyBuy.Api.Entity
{
    public class Item
    {
        public int Id { get; set; }
        public string ItemId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int MaxCount { get; set; }
        public string Url { get; set; }
    }
}
