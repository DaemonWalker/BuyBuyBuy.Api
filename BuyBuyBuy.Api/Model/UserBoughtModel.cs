namespace BuyBuyBuy.Api.Model
{
    public class UserBoughtModel
    {
        public ActivityModel Activity { get; set; }
        public ItemModel Item { get; set; }
        public long Quantity { get; set; }
        public string Time { get; set; }
    }
}
