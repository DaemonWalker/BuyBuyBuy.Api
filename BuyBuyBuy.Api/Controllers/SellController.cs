using BuyBuyBuy.Api.Model;
using BuyBuyBuy.Api.Service;
using Microsoft.AspNetCore.Mvc;
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
        public async ValueTask<IActionResult> BuyItem([FromBody] BuyItemModel model)
        {
            if (!await activityService.CheckActivityIsActive(model.ActivityId))
            {
                return BadRequest("该抢购不存在或者不在抢购时间内");
            }
            if (await itemService.BuyOneItem(model.ItemId))
            {
                return Ok("抢购成功");
            }
            else
            {
                return Ok("抢购失败");
            }
        }
    }
}
