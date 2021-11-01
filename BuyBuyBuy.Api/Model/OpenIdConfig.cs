using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Model
{
    public class OpenIdConfig
    {
        public string ServerUrl { get; set; }
        public string ClientId { get; set; }
        public List<string> Scopes { get; set; } = new List<string>();
        public List<string> RedirectUrls { get; set; } = new List<string>();
        public string ClientSecret { get; set; }
        public string GrantType { get; set; }
        public string Authority { get; set; }
        public string Scope => string.Join(' ', this.Scopes);
        public string RedirectUrl => string.Join(' ', this.RedirectUrls);
    }
}
