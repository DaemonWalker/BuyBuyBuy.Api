using BuyBuyBuy.Api.Contract.Data;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Middleware
{
    public class LimitRequestMiddleware
    {
        private const string AUTH_HEADER = @"Authorization";
        private readonly TimeSpan timeRange = TimeSpan.FromSeconds(3);
        private const int MaxRequestTime = 5;
        private readonly RequestDelegate _next;
        private readonly IRequestCache requestCache;
        public LimitRequestMiddleware(RequestDelegate next, IRequestCache requestCache)
        {
            this._next = next;
            this.requestCache = requestCache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;
            if (!path.Contains("/Sell/BuyItem"))
            {
                await _next(context);
                return;
            }
            if (context.Request.Headers.TryGetValue(AUTH_HEADER, out var token) == false)
            {
                await _next(context);
                return;
            }

            var key = token;
            await requestCache.ClearRequestAsync(key, timeRange);
            var requestTime = await requestCache.GetEarlistRequestAsync(key);
            if (!requestTime.HasValue || await requestCache.GetRequestCountAsync(key) < MaxRequestTime)
            {
                await requestCache.InsertRequestAsync(key);
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                await context.Response.WriteAsync("请勿短时间多次请求接口");
            }
        }
    }
}
