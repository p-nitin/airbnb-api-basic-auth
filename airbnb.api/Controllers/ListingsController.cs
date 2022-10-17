using airbnb.api.DataModel;
using airbnb.api.Extensions;
using airbnb.api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace airbnb.api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ListingsController : ControllerBase
    {
        private readonly ListingDataService _listingsService;

        public ListingsController(ListingDataService listingService) =>
            _listingsService = listingService;

        [HttpGet]
        [BasicAuthentication]
        public async Task<List<Listing>> Get(int? page, int? limit){

            if (!page.HasValue) page = 1;
            if (!limit.HasValue) limit = 5;

            int offset = limit.Value * (page.Value - 1);
            return await _listingsService.GetAsync(limit.Value, offset);
        }


        [HttpPost]
        public async Task<IActionResult> Post(Listing newListing)
        {
            await _listingsService.CreateAsync(newListing);
            return CreatedAtAction(nameof(Get), new { id = newListing.Id }, newListing);
        }

    }
}
