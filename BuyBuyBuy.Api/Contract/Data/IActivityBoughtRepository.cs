using BuyBuyBuy.Api.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Contract.Data
{
    public interface IActivityBoughtRepository
    {
        ValueTask AddUserBuyAsync(UserBuyHistory history);
        ValueTask<List<string>> GetActivityUsers(int actId);
        ValueTask<List<UserBuyHistory>> GetActivityUserBought(int actId, string userId);
    }
}
