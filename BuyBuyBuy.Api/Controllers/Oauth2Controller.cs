using BuyBuyBuy.Api.Model;
using BuyBuyBuy.Api.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class Oauth2Controller : ControllerBase
    {
        private readonly OpenIdService openIdService;
        public Oauth2Controller(OpenIdService openIdService)
        {
            this.openIdService = openIdService;
        }
        [HttpPost]
        public async Task<IActionResult> CallBack([FromForm] OpenIdCallback callback)
        {
            var userInfo = await openIdService.CreateJwt(callback);
            if (userInfo == default)
            {
                return BadRequest("登陆失败");
            }
            else
            {
                return Ok(userInfo);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GotoAuthPage([FromForm] string redirectUrl)
        {
            var url = await openIdService.GetAuthorizeUrl(redirectUrl);
            return Redirect(url);
        }
        [HttpPost]
        public IActionResult CreateToken([FromBody] string userId = "155775")
        {
            return Ok(openIdService.GenerateJWT(new UserModel() { Id = userId, EMail = "dhc-ce.ji@dhc.com.cn", Level = 2 }));
        }
    }
}
