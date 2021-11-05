using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Tools
{
    public static class UserExtension
    {
        public static string ToRole(this int level) => level > 1 ? "admin" : "user";
        public static int ToLevel(this string role) => role == "admin" ? 2 : 0;
    }
}
