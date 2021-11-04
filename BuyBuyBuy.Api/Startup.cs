using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Data;
using BuyBuyBuy.Api.Middleware;
using BuyBuyBuy.Api.Model;
using BuyBuyBuy.Api.Service;
using BuyBuyBuy.Api.Tools;
using FreeSql.Internal;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mysqlConf = this.Configuration.GetSection("mysql").Get<MySQLConfig>();
            var fsql = new FreeSql.FreeSqlBuilder()
               .UseConnectionString(FreeSql.DataType.MySql, mysqlConf.ConnectionString)
               .UseNameConvert(NameConvertType.ToLower)
               .UseAutoSyncStructure(true)
               .Build();

            var redisConfig = this.Configuration.GetSection("redis").Get<RedisConfig>().ToConfigOptions();
            services.Configure<OpenIdConfig>(this.Configuration.GetSection("openId"));

            services.AddSingleton(ConnectionMultiplexer.Connect(redisConfig));
            services.AddScoped(service => service.GetRequiredService<ConnectionMultiplexer>().GetDatabase());
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BuyBuyBuy.Api", Version = "v1" });
            });

            services.AddScoped<ItemService>()
                .AddScoped<ActivityService>()
                .AddScoped<ICache, Cache>()
                .AddSingleton<IActivityRepository, MySQLStore>()
                .AddSingleton<CurrentTimeAccessor>()
                .AddScoped<OpenIdService>()
                .AddScoped<IActivityBoughtRepository, RedisActivityBoughtRepository>()
                .AddSingleton<IItemRepository, MySQLStore>()
                .AddSingleton<IRequestCache, MemoryRequestCache>()
                .AddScoped<BoughtService>()
                .AddScoped<IActivityBoughtRepository, RedisActivityBoughtRepository>()
                .AddSingleton(fsql);

            services.AddHttpClient();
            services.AddSingleton<IDiscoveryCache>(services =>
            {
                var factory = services.GetRequiredService<IHttpClientFactory>();
                var config = services.GetRequiredService<IOptionsMonitor<OpenIdConfig>>().CurrentValue;
                return new DiscoveryCache(config.ServerUrl, () => factory.CreateClient(),
                    new DiscoveryPolicy() { RequireHttps = false });
            });

            var jwtConfig = this.Configuration.GetSection("jwt");
            services.Configure<JwtConfig>(jwtConfig);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                var token = jwtConfig.Get<JwtConfig>();
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    ValidIssuer = token.Issuer,
                    ValidAudience = token.Audience,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.FromSeconds(0),
                    ValidateLifetime = true
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BuyBuyBuy.Api v1"));
            }
            app.UseMiddleware<LimitRequestMiddleware>();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
