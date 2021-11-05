using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Entity;
using BuyBuyBuy.Api.Model;
using BuyBuyBuy.Api.Tools;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Service
{
    public class OpenIdService
    {
        private readonly HttpClient httpClient;
        private readonly IDiscoveryCache discovery;
        private readonly IOptionsMonitor<OpenIdConfig> config;
        private readonly IOptionsMonitor<JwtConfig> jwtConfig;
        private readonly IUserRepository userRepository;
        public OpenIdService(IHttpClientFactory httpClientFactory, IDiscoveryCache discovery, IUserRepository userRepository,
            IOptionsMonitor<OpenIdConfig> config, IOptionsMonitor<JwtConfig> jwtConfig)
        {
            this.httpClient = httpClientFactory.CreateClient("OpenId");
            this.discovery = discovery;
            this.config = config;
            this.jwtConfig = jwtConfig;
            this.userRepository = userRepository;
        }

        public async ValueTask<string> GetAuthorizeUrl(string redirectUrl = null)
        {
            if (string.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = this.config.CurrentValue.RedirectUrl;
            }
            var disco = await this.discovery.GetAsync();
            var ru = new RequestUrl(disco.AuthorizeEndpoint);
            var url = ru.CreateAuthorizeUrl(clientId: this.config.CurrentValue.ClientId, responseType: this.config.CurrentValue.GrantType,
                scope: this.config.CurrentValue.Scope, redirectUri: redirectUrl);
            return url;
        }

        public async ValueTask<UserModel> CreateJwt(OpenIdCallback callback)
        {
            var disco = await this.discovery.GetAsync();
            var res = await this.httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest()
            {
                Address = disco.TokenEndpoint,
                ClientId = this.config.CurrentValue.ClientId,
                ClientSecret = this.config.CurrentValue.ClientSecret,
                RedirectUri = string.IsNullOrWhiteSpace(callback.RedirectUrl) ? this.config.CurrentValue.RedirectUrl : callback.RedirectUrl,
                Code = callback.Code,
                ClientCredentialStyle = ClientCredentialStyle.PostBody,
            });

            var userInfo = await this.httpClient.GetUserInfoAsync(new UserInfoRequest()
            {
                Address = disco.UserInfoEndpoint,
                ClientId = this.config.CurrentValue.ClientId,
                ClientSecret = this.config.CurrentValue.ClientSecret,
                Token = res.AccessToken,
            });
            var userId = userInfo.Claims.First(p => p.Type == "sub").Value.ToString();
            UserModel user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                var name = userInfo.Claims.First(p => p.Type == "name").Value.ToString();
                user = await userRepository.CreateOrUpdateUser(new User() { Id = userId, Level = 1, Name = name });
            }
            user.Token = GenerateJWT(user);
            return user;
        }

        public string GenerateJWT(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtConfig.CurrentValue.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Sid, user.Id), new Claim(ClaimTypes.Role, user.Role.ToLevel().ToString()) }),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtConfig.CurrentValue.Issuer,
                Audience = jwtConfig.CurrentValue.Audience,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
