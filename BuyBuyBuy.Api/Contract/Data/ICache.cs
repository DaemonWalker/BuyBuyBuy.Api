using BuyBuyBuy.Api.Entity;
using BuyBuyBuy.Api.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Contract.Data
{
    public interface ICache
    {
        Task<long> AddUserBuyAsync(BuyItemModel buy);
        Task<long> BuyItemAsync(BuyItemModel buy);
        Task SetActivityInventory(List<ActivityItem> items);
    }
}
