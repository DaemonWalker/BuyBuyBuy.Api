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
        public int Level { get; set; }
        public string Token { get; set; }
    }
}
