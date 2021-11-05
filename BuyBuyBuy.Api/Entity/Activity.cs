using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;

namespace BuyBuyBuy.Api.Entity
{
    [Table(Name = "activity")]
    public class Activity
    {
        [Column(IsIdentity = true, IsPrimary = true, Name = "id")]
        public int Id { get; set; }

        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "start")]
        public DateTime Start { get; set; }

        [Column(Name = "end")]
        public DateTime End { get; set; }
    }
}
