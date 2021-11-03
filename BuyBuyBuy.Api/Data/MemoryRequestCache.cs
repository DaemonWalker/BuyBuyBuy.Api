using BuyBuyBuy.Api.Contract.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyBuyBuy.Api.Data
{
    public class MemoryRequestCache : IRequestCache
    {
        private readonly ConcurrentDictionary<string, ConcurrentQueue<DateTime>> requests =
            new ConcurrentDictionary<string, ConcurrentQueue<DateTime>>();

        public ValueTask ClearRequestAsync(string key, TimeSpan timeSpan)
        {
            var now = DateTime.Now;
            var queue = requests.GetValueOrDefault(key);
            if (queue == default)
            {
                return ValueTask.CompletedTask;
            }
            var remove = queue.TryPeek(out var dt);
            while (remove && (now - dt) > timeSpan)
            {
                remove = queue.TryDequeue(out dt);
            }
            return ValueTask.CompletedTask;
        }

        public ValueTask<DateTime?> GetEarlistRequestAsync(string key)
        {
            if (!requests.TryGetValue(key, out var dts))
            {
                return ValueTask.FromResult((DateTime?)null);
            }
            if (dts.Any() && dts.TryPeek(out var dt))
            {
                return ValueTask.FromResult((DateTime?)dt);
            }
            else
            {
                return ValueTask.FromResult((DateTime?)null);
            }
        }

        public ValueTask InsertRequestAsync(string key)
        {
            var queue = requests.GetOrAdd(key, new ConcurrentQueue<DateTime>());
            queue.Enqueue(DateTime.Now);
            return ValueTask.CompletedTask;
        }
        public ValueTask<int> GetRequestCountAsync(string key)
        {
            var queue = requests.GetValueOrDefault(key);
            if (queue == default)
            {
                return ValueTask.FromResult(0);
            }
            else
            {
                return ValueTask.FromResult(queue.Count);
            }
        }
    }
}
