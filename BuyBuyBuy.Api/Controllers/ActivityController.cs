using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Model;
using BuyBuyBuy.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ActivityController : ControllerBase
    {
        private readonly ItemService itemService;
        private readonly ActivityService activityService;
        public ActivityController(ItemService itemService, ActivityService activityService)
        {
            this.itemService = itemService;
            this.activityService = activityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItems([FromQuery] int actId)
        {
            var list = await itemService.GetItemsByActivity(actId);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> InitInventory([FromBody] int actId)
        {
            await activityService.InitInventory(actId);
            return Ok("初始化完成");
        }
    }
}
