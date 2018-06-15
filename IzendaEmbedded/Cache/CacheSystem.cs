using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace IzendaEmbedded.Cache
{
    public class CacheSystem
    {
        private static readonly MemoryCache Cache;
        static CacheSystem()
        {
            Cache = new MemoryCache("CacheSystem");
        }

        public static object GetCacheItem(string key)
        {
            var storedItem = Cache.GetCacheItem(key);
            return storedItem?.Value;
        }

        public static void AddCacheItem(string key, object obj, int minutesToCache)
        {
            if (obj != null)
            {
                Cache.Add(key, obj, DateTimeOffset.Now.AddMinutes(minutesToCache));
            }
        }

        public static void RemoveCacheItem(string key)
        {
            Cache.Remove(key);
        }
    }
}