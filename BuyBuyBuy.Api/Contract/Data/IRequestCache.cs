using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Contract.Data
{
    public interface IRequestCache
    {
        ValueTask InsertRequestAsync(string key);
        ValueTask<DateTime?> GetEarlistRequestAsync(string key);
        ValueTask ClearRequestAsync(string key, TimeSpan timeSpan);
        ValueTask<int> GetRequestCountAsync(string key);
    }
}
