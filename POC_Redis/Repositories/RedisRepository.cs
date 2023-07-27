using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace POC_Redis.Repositories
{
    public class RedisRepository : ICacheRepository
    {
        private readonly IDistributedCache distributedCache;

        public RedisRepository(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache ?? 
                throw new ArgumentNullException(nameof(distributedCache));
        }

        public async Task<IEnumerable<T>> GetCollection<T>(string collectionKey)
        {
            var result = await distributedCache.GetStringAsync(collectionKey);

            if (result == null)
            {
                return default;
            }

            return JsonConvert.DeserializeObject<IEnumerable<T>>(result);
        }

        public async Task<T> GetValue<T>(Guid id)
        {
            var key = id.ToString().ToLower();

            var result = await distributedCache.GetStringAsync(key);

            if (result == null)
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(result);
        }

        public Task SetCollection<T>(string collectionKey, IEnumerable<T> collection)
        {
            throw new NotImplementedException();
        }

        public async Task SetValue<T>(Guid id, T obj)
        {
            var key = id.ToString().ToLower();
            var newValue = JsonConvert.SerializeObject(obj);

            await distributedCache.SetStringAsync(key, newValue);
        }
    }
}
