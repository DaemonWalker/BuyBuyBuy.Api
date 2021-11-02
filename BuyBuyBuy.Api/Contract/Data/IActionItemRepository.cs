using BuyBuyBuy.Api.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Contract.Data
{
    public interface IActionItemRepository
    {
        Task<List<ActivityItem>> GetActivityItemsAsync(int actId);
        Task<ActivityItem> GetOneItemInActivityAsync(int actId, string itemId);
    }
}
