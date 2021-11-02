using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Constance
{
    public enum BuyItemResult
    {
        OK,
        [Description("您已经买过了")]
        Buyed,
        [Description("运气不好，已经被抢光了")]
        SoldOut,
        [Description("库存不足，以为您抢到了最大数量")]
        NotEnough,
        [Description("购买数量超过个人单品上限，已为您自动抢购了剩余商品")]
        OverLimit,
    }
}
