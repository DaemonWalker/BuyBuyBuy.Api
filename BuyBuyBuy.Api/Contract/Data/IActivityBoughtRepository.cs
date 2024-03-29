﻿using BuyBuyBuy.Api.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Contract.Data
{
    public interface IActivityBoughtRepository
    {
        Task AddUserBuyAsync(UserBought history);
        Task<List<string>> GetActivityUsers(int actId);
        Task<List<UserBought>> GetActivityUserBought(int actId, string userId);
    }
}
