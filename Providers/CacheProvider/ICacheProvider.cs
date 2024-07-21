using Microsoft.Extensions.Caching.Memory;

namespace WebApiDotnetCoreSample.Providers.CacheProvider
{
    public interface ICacheProvider
    {
        public object GetValue (string key);

        public void SetValue (string key, object value);

        public void RemoveEntry(string key);
    }
}
