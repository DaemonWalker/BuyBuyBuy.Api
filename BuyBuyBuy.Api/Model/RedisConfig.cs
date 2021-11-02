using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Model
{
    public class RedisConfig
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public int DB { get; set; }
        public ConfigurationOptions ToConfigOptions()
        {
            var config = ConfigurationOptions.Parse($"{Address}:{Port}");
            config.Password = this.Password;
            config.DefaultDatabase = this.DB;
            return config;
        }
    }
}
