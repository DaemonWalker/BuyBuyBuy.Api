using BuyBuyBuy.Api.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Contract.Data
{
    public interface IActivityRepository
    {
        ValueTask<Activity> GetActivityByIdAsync(int Id);
    }
}
