using BuyBuyBuy.Api.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Contract.Data
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);
        Task<User> CreateOrUpdateUser(User user);
    }
}
