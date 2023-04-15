using JSONanalyser.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace JSONanalyser.Service
{
    public class DataService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<DataService> _logger;

        public DataService(HttpClient httpClient, IMemoryCache memoryCache, ILogger<DataService> logger)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
            _logger = logger;
        }
        public async Task<List<Beer>> GetAllBeersAsync(string url)
        {
            var cachedBeers = _memoryCache.Get<List<Beer>>("beers");

            if (cachedBeers != null)
            {
                _logger.LogInformation("Retrieved beers from cache");
                return cachedBeers;
            }

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to retrieve data from URL");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var beers = JsonConvert.DeserializeObject<List<Beer>>(content);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

            _memoryCache.Set("beers", beers, cacheOptions);

            _logger.LogInformation("Retrieved beers from URL");

            return beers;
        }
    }
}
