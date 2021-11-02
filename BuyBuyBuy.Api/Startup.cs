using BuyBuyBuy.Api.Contract.Data;
using BuyBuyBuy.Api.Data;
using BuyBuyBuy.Api.Model;
using BuyBuyBuy.Api.Service;
using BuyBuyBuy.Api.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var redisConfig = this.Configuration.GetSection("redis").Get<RedisConfig>().ToConfigOptions();

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
                .AddSingleton<IStore, StaticStore>()
                .AddSingleton<CurrentTimeAccessor>()
                .AddScoped<IActivityHistory, RedisActivityHistory>()
                .AddScoped<IActionItemRepository, StaticStore>();
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
