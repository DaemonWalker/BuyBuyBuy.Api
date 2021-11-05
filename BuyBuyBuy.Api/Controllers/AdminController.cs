using BuyBuyBuy.Api.Entity;
using BuyBuyBuy.Api.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IFreeSql fsql;
        private readonly ActivityService activityService;
        private readonly BoughtService boughtService;
        public AdminController(IFreeSql fsql, ActivityService activityService, BoughtService boughtService)
        {
            this.fsql = fsql;
            this.activityService = activityService;
            this.boughtService = boughtService;
        }

        [HttpPost]
        public async ValueTask<IActionResult> Init()
        {
            await fsql.Insert(new Item() { Id = "cherry-keyboard", Name = "樱桃机械键盘", Url = "http://192.168.12.2/s/PC9dRNt4n2QwZ2n/preview" })
                .ExecuteAffrowsAsync();
            await fsql.Insert(new Activity() { Name = "测试秒杀", Start = DateTime.Now.AddDays(-100), End = DateTime.Now.AddDays(100) })
                .ExecuteAffrowsAsync();

            await fsql.Select<User>().ToListAsync();
            var items = await fsql.Select<Item>().ToListAsync();
            var acts = await fsql.Select<Activity>().ToListAsync();
            await fsql.Select<ActivityItem>().ToListAsync();
            await fsql.Select<UserBought>().ToListAsync();


            await fsql.Insert(new ActivityItem() { ActivityId = acts.First().Id, ItemId = items.First().Id, Inventory = 5, UserLimit = 3, Price = 1.5f })
                .ExecuteAffrowsAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> InitInventory([FromBody] int actId)
        {
            await activityService.InitInventory(actId);
            return Ok("初始化完成");
        }

        [HttpPost]
        public async ValueTask<IActionResult> GetAllBought([FromBody] int actId)
        {
            var bought = await boughtService.GetActivityBought(actId);
            return Ok(bought);
        }
    }
}
