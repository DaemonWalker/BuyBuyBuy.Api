using BuyBuyBuy.Api.Constance;
using BuyBuyBuy.Api.Model;
using BuyBuyBuy.Api.Service;
using BuyBuyBuy.Api.Tools;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SellController : ControllerBase
    {
        private readonly ItemService itemService;
        private readonly ActivityService activityService;
        public SellController(ItemService itemService, ActivityService activityService)
        {
            this.itemService = itemService;
            this.activityService = activityService;
        }

        [HttpPost]
        public async ValueTask<IActionResult> BuyItem([FromBody] BuyItemModel model)
        {
            if (!await activityService.CheckActivityIsActive(model.ActivityId))
            {
                return BadRequest("该抢购不存在或者不在抢购时间内");
            }
            if (model.Quantity == 0)
            {
                model.Quantity = 1;
            }
            var result = await itemService.BuyOneItem(model);
            return Ok(result.GetEnumDescription());
        }
    }
}
