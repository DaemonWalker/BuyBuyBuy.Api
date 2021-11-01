using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Model
{
    public class OpenIdCallback
    {
        public string Code { get; set; }
        public string Scope { get; set; }
        public string SessionState { get; set; }
        public string RedirectUrl { get; set; }
    }
}
