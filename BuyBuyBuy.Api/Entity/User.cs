using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Entity
{
    [Table(Name = "user")]
    public class User
    {
        [Column(Name = "user", IsIdentity = true)]
        public string Id { get; set; }

        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "level")]
        public int Level { get; set; }
    }
}
