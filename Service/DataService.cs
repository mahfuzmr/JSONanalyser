using JSONanalyser.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using System.Web;

namespace JSONanalyser.Service
{
    public interface IDataService
    {
        /// <summary>
        /// Decode the URL and extract the base URL from the string that might have where %2F represents which represents /,
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Base url</returns>
        string UrlDecode(string url);
        /// <summary>
        /// Get the data from the provided url and act as a backend database. [Base data used by service itself]
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Data object as alist</returns>
        Task<List<Beer>> GetAllBeersAsync(string url);
        /// <summary>
        /// Get the bears from the provided url and return the most expensive bear.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>bear object </returns>
        Task<Beer> GetTheMostExpensive(string url);
        /// <summary>
        /// Get the bears from the provided url and return the Cheapest bear.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>bear object </returns>
        Task<Beer> GetTheMostCheapest(string url);
        /// <summary>
        /// Get the bears from the provided url and return the bear with soecific price.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="amount"></param>
        /// <returns>bear object </returns>
        Task<List<Beer>> GetbyExactAmount(double amount, string url);
    }

    public class DataService : IDataService
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
            var cachedBeers = _memoryCache.Get<List<Beer>>("beerData");

            if (cachedBeers != null)
            {
                _logger.LogInformation("Retrieved beers from cache");
                return cachedBeers;
            }
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var beers = await response.Content.ReadFromJsonAsync<List<Beer>>();
                return beers;
            }
            else
            {
                _logger.LogError("Failed to retrieve data from URL");
                return null;
            }

        }

        public async Task<List<Beer>> GetbyExactAmount(double amount, string url)
        {
            var beerData = await GetAllBeersAsync(url);
            var beerByPrice = beerData.SelectMany(b => b.Articles).Where(p => p.Price.ToString() == amount.ToString()).OrderBy(a => a.PricePerUnit).ToList();
            List<Beer> beerList = new List<Beer>();

            foreach (var item in beerByPrice)
            {
                // Find the first Beer object in beerData that contains the current item
                Beer matchingBeer = beerData.FirstOrDefault(i => i.Articles.Contains(item));

                // If we found a matching beer, create a new Beer object and add it to the list
                if (matchingBeer != null)
                {
                    Beer beerObject = new Beer
                    {
                        Id = matchingBeer.Id,
                        BrandName = matchingBeer.BrandName,
                        Name = matchingBeer.Name,
                        Articles = new List<Article> {
                            new Article
                            {
                                Id = item.Id,
                                ShortDescription = item.ShortDescription,
                                Price = item.Price,
                                Unit = item.Unit,
                                PricePerUnitText = item.PricePerUnitText,
                                Image = item.Image
                            }
                        }
                    };

                    // Check if the beer is already in the list
                    Beer matchingBeerInList = beerList.FirstOrDefault(p => p.Id == beerObject.Id);
                    if (matchingBeerInList != null)
                    {
                        // If it is, add the new article to the Articles list
                        matchingBeerInList.Articles.Add(beerObject.Articles.First());
                    }
                    else
                    {
                        // If it's not, add the whole Beer object to the list
                        beerList.Add(beerObject);
                    }
                }

            }
            return beerList;
        }

        public async Task<Beer> GetTheMostCheapest(string url)
        {
            var beerData = await GetAllBeersAsync(url);

            var cheapestBeer = beerData.SelectMany(b => b.Articles).OrderBy(a => a.PricePerUnit).FirstOrDefault();
            var responseData = new Beer
            {
                //[TODO: Find the parent]
                Id = beerData.Where(i => i.Articles.Contains(cheapestBeer)).First().Id,
                BrandName = beerData.Where(i => i.Articles.Contains(cheapestBeer)).First().BrandName,
                Name = beerData.Where(i => i.Articles.Contains(cheapestBeer)).First().Name,
                Article = new Article
                {
                    Id = cheapestBeer.Id,
                    ShortDescription = cheapestBeer.ShortDescription,
                    Price = cheapestBeer.Price,
                    Unit = cheapestBeer.Unit,
                    PricePerUnitText = cheapestBeer.PricePerUnitText,
                    Image = cheapestBeer.Image
                }
            };
            return responseData;
        }

        public async Task<Beer> GetTheMostExpensive(string url)
        {
            List<Beer> beerData = await GetAllBeersAsync(url);
            var mostExpensiveBeer = beerData.SelectMany(b => b.Articles).OrderByDescending(a => a.PricePerUnit).FirstOrDefault();

            var responseData = new Beer
            {
                //[TODO: Find the parent]
                Id = beerData.Where(i => i.Articles.Contains(mostExpensiveBeer)).First().Id,
                BrandName = beerData.Where(i => i.Articles.Contains(mostExpensiveBeer)).First().BrandName,
                Name = beerData.Where(i => i.Articles.Contains(mostExpensiveBeer)).First().Name,
                Article = new Article
                {
                    Id = mostExpensiveBeer.Id,
                    ShortDescription = mostExpensiveBeer.ShortDescription,
                    Price = mostExpensiveBeer.Price,
                    Unit = mostExpensiveBeer.Unit,
                    PricePerUnitText = mostExpensiveBeer.PricePerUnitText,
                    Image = mostExpensiveBeer.Image
                }
            };
            return responseData;
        }

        public string UrlDecode(string url)
        {
            var decodedUrl = HttpUtility.UrlDecode(url);
            var baseUrl = new Uri(decodedUrl).GetLeftPart(UriPartial.Authority);
            return decodedUrl; // need the complete url

        }
    }
}
