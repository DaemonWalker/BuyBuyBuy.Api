using BuyBuyBuy.Api.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Model
{
    public class ActivityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public bool IsStart { get; set; }
        public DateTime StartDate { get; set; }

        public static implicit operator ActivityModel(Activity activity)
        {
            if (activity == default)
            {
                return default;
            }
            return new ActivityModel()
            {
                Id = activity.Id,
                Name = activity.Name,
                Start = activity.Start.ToString("yyyy/MM/dd HH:mm:ss"),
                End = activity.End.ToString("yyyy/MM/dd HH:mm:ss"),
                StartDate = activity.Start,
            };
        }
    }
}
