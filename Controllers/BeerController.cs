using JSONanalyser.Models;
using JSONanalyser.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;

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

    }
}
