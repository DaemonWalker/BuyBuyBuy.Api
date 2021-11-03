using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Tools
{
    public static class DateTimeExtension
    {
        public static long ToNumber(this DateTime dt) =>
            dt.Year * 10000000000 + dt.Month * 100000000 + dt.Day * 1000000 +
            dt.Hour * 10000 + dt.Minute * 100 + dt.Second;
        public static DateTime ToDateTime(this long dt)
        {
            var year = (int)(dt / 10000000000);
            dt %= 10000000000;
            var month = (int)(dt / 100000000);
            dt %= 100000000;
            var day = (int)(dt / 1000000);
            dt %= 1000000;
            var hour = (int)(dt / 10000);
            dt %= 10000;
            var min = (int)(dt / 100);
            var sec = (int)(dt % 100);
            return new DateTime(year, month, day, hour, min, sec);
        }
    }
}
