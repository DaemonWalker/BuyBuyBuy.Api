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

        [HttpPost]
        public async Task<IActionResult> GetAllItems([FromBody] int actId)
        {
            var list = await itemService.GetItemsByActivity(actId);
            return Ok(list);
        }

        [HttpGet]
        public async ValueTask<IActionResult> GetCurrentActivity()
        {
            ActivityModel act = await activityService.GetNextActivityAsync();
            return Ok(act);
        }

        public async ValueTask<IActionResult> GetAllActivities()
        {
            var acts = await activityService.GetAllActivities();
            return Ok(acts);
        }

    }
}
