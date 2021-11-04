using BuyBuyBuy.Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace BuyBuyBuy.Api.Tools
{
    public static class ControllerExtension
    {
        public static string GetUserId(this ControllerBase controlller)
        {
            var sid = controlller.HttpContext.User.FindFirst(ClaimTypes.Sid);
            return sid?.Value ?? String.Empty;
        }
        public static UserModel GetUserInfo(this ControllerBase controller)
        {
            var userId = controller.GetUserId();
            var name = controller.HttpContext.User.FindFirst(ClaimTypes.Name);
            var level = controller.HttpContext.User.FindFirst(ClaimTypes.Role);
            return new UserModel()
            {
                Name = name?.Value ?? String.Empty,
                Id = userId,
                Level = Convert.ToInt32(level?.Value ?? "1"),
            };
        }
    }
}
