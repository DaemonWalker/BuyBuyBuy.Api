using BuyBuyBuy.Api.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Contract.Data
{
    public interface IActivityRepository
    {
        Task<Activity> GetActivityByIdAsync(int Id);
        Task<Activity> GetLiveActivityAsync();
        Task<Activity> GetNextHourActivityAsync();
        Task<List<Activity>> GetAll();
    }
}
