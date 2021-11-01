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
        private readonly IMemoryCache memoryCache;
        private readonly ZentaoClient zentaoClient;
        private readonly MemoryCacheCancellationTokenSource cacheToken;
        public Oauth2Controller(OpenIdService openIdService, IMemoryCache memoryCache, ZentaoClient zentaoClient, MemoryCacheCancellationTokenSource cacheToken)
        {
            this.openIdService = openIdService;
            this.memoryCache = memoryCache;
            this.zentaoClient = zentaoClient;
            this.cacheToken = cacheToken;
        }
        [HttpPost]
        public async Task<IActionResult> CallBack([FromForm] OpenIdCallback callback)
        {
            var user = await openIdService.CreateZentaoUser(callback);
            if (await zentaoClient.IsUserExists(user.Account) == false)
            {
                if (memoryCache.TryGetValue(user.Account, out var _) == false)
                {
                    var options = new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(cacheToken.Token));

                    memoryCache.Set(user.Account, user.Account, options);
                    await zentaoClient.CreateUser(user);
                }
                else
                {
                    return BadRequest("账号初始化中，请刷新网页");
                }
            }
            else
            {
                memoryCache.Remove(user.Account);
            }

            return Redirect(zentaoClient.GetLoginUrl(user.Account));
        }

        [HttpPost]
        public async Task<IActionResult> GotoAuthPage([FromForm] string redirectUrl)
        {
            var url = await openIdService.GetAuthorizeUrl(redirectUrl);
            return Redirect(url);
        }
    }
}
