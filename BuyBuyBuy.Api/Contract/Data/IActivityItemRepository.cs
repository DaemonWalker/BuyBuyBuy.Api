using BuyBuyBuy.Api.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Contract.Data
{
    public interface IActivityItemRepository
    {
        Task<List<ActivityItem>> GetItemsAsync(int actId);
        Task<ActivityItem> GetItemByIdAsync(int actId, string itemId);
    }
}
