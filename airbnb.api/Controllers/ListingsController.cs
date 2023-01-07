using airbnb.api.DataModel;
using airbnb.api.Extensions;
using airbnb.api.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

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
        public async Task<JsonResult> Get([FromQuery] QueryFilter filter, int? page, int? limit, string? fields = null, string? sort = null)
        {
            if (!page.HasValue) page = 1;
            if (!limit.HasValue) limit = 5;

            int offset = limit.Value * (page.Value - 1);
            var listings = await _listingsService.GetAsync(filter, sort,limit.Value, offset);
            listings.ForEach((item) =>
            {
                item.SetSerializableProperties(fields);
            });
            return new JsonResult(listings, new Newtonsoft.Json.JsonSerializerSettings()
            {
                ContractResolver = new FieldsFilterContractResolver()
            });

        }

        [HttpPost]
        public async Task<IActionResult> Post(Listing newListing)
        {
            await _listingsService.CreateAsync(newListing);
            return CreatedAtAction(nameof(Get), new { id = newListing.Id }, newListing);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, Listing updatedListing)
        {
            if (id != updatedListing.Id)
            {
                return BadRequest();
            }
            await _listingsService.UpdateAsync(id,updatedListing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var listing = _listingsService.GetAsync(id);
            if(listing == null)
            {
                return NotFound();
            }
            await _listingsService.RemoveAsync(id);

            return NoContent();
        }

    }
}
