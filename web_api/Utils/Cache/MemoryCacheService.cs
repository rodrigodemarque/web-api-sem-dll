using System;
using System.Runtime.Caching;
using web_api.Interfaces;

namespace web_api.Utils.Cache
{
    public class MemoryCacheService : ICacheService
    {

        readonly ObjectCache cache;
        public MemoryCacheService() 
        {
            cache = MemoryCache.Default;
        }

        public T Get<T>(string key)
        {
            return (T) cache.Get(key);
        }

        public void Set<T>(string key, T value, int cacheSeconds)
        {
            //CacheItemPolicy policy = new CacheItemPolicy();
            //policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cacheMinutes);         
            //cache.Set(key, value, policy);
            cache.Set(key, value, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cacheSeconds) });
        }

        //public void Set<T>(string key, T value)
        //{
        //   Set(key, value, 900);
        //}
        
        public void Remove(string key)
        {
                cache.Remove(key);
        }

    }
}