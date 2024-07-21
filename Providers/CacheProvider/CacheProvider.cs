using Microsoft.Extensions.Caching.Memory;
using WebApiDotnetCoreSample.DataStoreModel;

namespace WebApiDotnetCoreSample.Providers.CacheProvider
{
    public class CacheProvider : ICacheProvider
    {
        private readonly IMemoryCache memoryCache;
        public CacheProvider(IMemoryCache cache) 
        {
            this.memoryCache = cache;
        }

        public object GetValue(string key)
        {
            this.memoryCache.TryGetValue(key, out var value);

            return value;
        }

        public void SetValue(string key, object value)
        {
           this.memoryCache.Set<object>(key, value, TimeSpan.FromMinutes(60));
        }

        public void RemoveEntry(string key)
        {
            this.memoryCache.Remove(key);
        }
    }
}
