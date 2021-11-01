using System;

namespace BuyBuyBuy.Api.Tools
{
    public class CurrentTimeAccessor
    {
        const string ZONE_NAME = "China Standard Time";
        public DateTime Access()
        {
            var utcdate = DateTime.Now.ToUniversalTime();
            var zone = TimeZoneInfo.FindSystemTimeZoneById(ZONE_NAME);
            return TimeZoneInfo.ConvertTimeFromUtc(utcdate, zone);
        }
    }
}
