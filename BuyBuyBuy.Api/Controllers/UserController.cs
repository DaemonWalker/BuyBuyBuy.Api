using BuyBuyBuy.Api.Service;
using BuyBuyBuy.Api.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly BoughtService boughtService;
        public UserController(BoughtService boughtService)
        {
            this.boughtService = boughtService;
        }
        [HttpPost]
        public async ValueTask<IActionResult> GetMyBought([FromBody] int actId)
        {
            var userId = this.GetUserId();
            var bought = await boughtService.GetActivityUserBought(actId, userId);
            return Ok(bought);
        }

    }
}
