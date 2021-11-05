using BuyBuyBuy.Api.Entity;
using BuyBuyBuy.Api.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Model
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }

        public static implicit operator User(UserModel user) =>
            user == default ? default : new User()
            {
                Id = user.Id,
                Level = user.Role.ToLevel(),
                Name = user.Name,
            };

        public static implicit operator UserModel(User user) =>
            user == default ? default : new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                Role = user.Level.ToRole(),
            };
    }
}
