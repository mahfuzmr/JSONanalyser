using JSONanalyser.Models;
using JSONanalyser.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using System;

namespace JSONanalyser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeerController : ControllerBase
    {
        private readonly DataService _beerService;
        private readonly ILogger<BeerController> _logger;

        public BeerController(DataService beerService, ILogger<BeerController> logger)
        {
            _beerService = beerService;
            _logger = logger;
        }

        [HttpGet("most-expensive")]
        [Produces("application/json")]
        public async Task<ActionResult> GetMostExpensive(string url)
        {
            
            var baseUrl =  _beerService.UrlDecode(url);
            var beers = await _beerService.GetTheMostExpensive(baseUrl);
            if (beers == null)
            {
                return NotFound(new JsonResult(new object()));
            }

            return Ok(new JsonResult(value: beers));
        }
        [HttpGet("most-cheapest")]
        [Produces("application/json")]
        public async Task<ActionResult> GetMostCheapest(string url)
        {

            var baseUrl = _beerService.UrlDecode(url);
            var beers = await _beerService.GetTheMostCheapest(baseUrl);
            if (beers == null)
            {
                return NotFound(new JsonResult(new object()));
            }

            return Ok(new JsonResult(value: beers));
        }
        [HttpGet("exact-amount/{amount:double}")]
        [Produces("application/json")]
        public async Task<ActionResult> GetExactAmount(double amount, string url)
        {

            var baseUrl = _beerService.UrlDecode(url);
            var beers = await _beerService.GetbyExactAmount(amount, url);
            if (beers == null)
            {
                return NotFound(new JsonResult(new object()));
            }

            return Ok(new JsonResult(value: beers));
        }
        [HttpGet("most-count")]
        [Produces("application/json")]
        public async Task<ActionResult> GetMaxCountBottles(string url)
        {

            var baseUrl = _beerService.UrlDecode(url);
            var beers = await _beerService.GetMostBottleCount( url);
            if (beers == null)
            {
                return NotFound(new JsonResult(new object()));
            }

            return Ok(new JsonResult(value: beers));
        }

        [HttpGet("all-question")]
        [Produces("application/json")]
        public async Task<ActionResult> GetAnswers(string url)
        {

            var baseUrl = _beerService.UrlDecode(url);

            var beerProduct_1 = await _beerService.GetTheMostExpensive(baseUrl);
            var beerProduct_2 = await _beerService.GetTheMostCheapest(baseUrl);
            var beerProduct_3 = await _beerService.GetbyExactAmount(17.99D,baseUrl);
            var beerProduct_4 = await _beerService.GetMostBottleCount(baseUrl);

       


            var combinedObject = new
            {
                Object1 = beerProduct_1,
                Object2 = beerProduct_2,
                Object3 = beerProduct_4,
                List = beerProduct_3
            };
            string json = JsonConvert.SerializeObject(combinedObject);

            // convert the combined object to JSON and return it
            return Ok(new JsonResult(value: json));
        }
    }
}
