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
using System.Xml.Linq;
using System.Text.Json.Serialization;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetMostExpensive(string url)
        {

            var baseUrl = _beerService.UrlDecode(url);
            var beers = await _beerService.GetTheMostExpensive(baseUrl);
            if (beers == null)
            {
                return NotFound(new JsonResult(new object()));
            }
            // return Ok(new JsonResult(value: beers));

            var result = new JsonResult(beers)

            {
                StatusCode = 200,
                ContentType = "application/json"
            };

            return new OkObjectResult(result);
        }
        [HttpGet("most-cheapest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetMostCheapest(string url)
        {

            var baseUrl = _beerService.UrlDecode(url);
            var beers = await _beerService.GetTheMostCheapest(baseUrl);
            if (beers == null)
            {
                return NotFound(new JsonResult(new object()));
            }

            // return Ok(new JsonResult(value: beers));

            var result = new JsonResult(beers)
            {
                StatusCode = 200,
                ContentType = "application/json"
            };

            return new OkObjectResult(result);
        }
        [HttpGet("exact-amount/{amount:double}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetExactAmount(double amount, string url)
        {

            var baseUrl = _beerService.UrlDecode(url);
            var beers = await _beerService.GetbyExactAmount(amount, url);
            if (beers == null)
            {
                return NotFound(new JsonResult(new object()));
            }
           
            // return Ok(new JsonResult(value: beers));

            var result = new JsonResult(beers)
            {
                StatusCode = 200,
                ContentType = "application/json"
            };

            return new OkObjectResult(result);
        }
        [HttpGet("most-count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetMaxCountBottles(string url)
        {

            var baseUrl = _beerService.UrlDecode(url);
            var beers = await _beerService.GetMostBottleCount(url);
            if (beers == null)
            {
                return NotFound(new JsonResult(new object()));
            }

          
            // return Ok(new JsonResult(value: beers));

            var result = new JsonResult(beers)
            {
                StatusCode = 200,
                ContentType = "application/json"
            };

            return new OkObjectResult(result);
        }

        [HttpGet("all-question")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAnswers(string url)
        {

            var baseUrl = _beerService.UrlDecode(url);            

            var beerProduct_1 = await _beerService.GetTheMostExpensive(baseUrl);
            var beerProduct_2 = await _beerService.GetTheMostCheapest(baseUrl);
            var beerProduct_3 = await _beerService.GetbyExactAmount(17.99D, baseUrl);
            var beerProduct_4 = await _beerService.GetMostBottleCount(baseUrl);

            var DTO = new
            {
                MostExpensive = beerProduct_1,
                MostCheapest = beerProduct_2,
                ExactAmount = beerProduct_3,
                MostBottleCount = beerProduct_4,

            };
            // return Ok(new JsonResult(value: jsonPayload));
            // Return as JSON Payload with appropreate information
            var result = new JsonResult(DTO)
            {
                StatusCode = 200,
                ContentType = "application/json"               
            };

            return new OkObjectResult(result);
        }
    }
}
