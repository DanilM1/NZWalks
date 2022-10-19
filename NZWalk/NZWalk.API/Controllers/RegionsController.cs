using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalk.API.Repositories;

namespace NZWalk.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepositry regionRepositry;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepositry regionRepositry, IMapper mapper)
        {
            this.regionRepositry = regionRepositry;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles ="reader")]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepositry.GetAllAsync();

            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepositry.GetAsync(id);

            if (region == null) return NotFound();

            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            // Validate The Request
            // if (!ValidateAddRegionAsync(addRegionRequest)) return BadRequest(ModelState);

            // Request(DTO) to Domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population
            };

            // Pass details to Repository
            region = await regionRepositry.AddAsync(region);

            // Convert back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            var region = await regionRepositry.DeleteAsync(id);

            if (region == null) return NotFound();

            return Ok(mapper.Map<Models.DTO.Region>(region));
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            // Validate The Request
            // if (!ValidateUpdateRegionAsync(updateRegionRequest)) return BadRequest(ModelState);

            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population
            };

            region = await regionRepositry.UpdateAsync(id, region);

            if (region == null) return NotFound();

            return Ok(mapper.Map<Models.DTO.Region>(region));
        }

        #region Private methods

        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if (addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest), "Add Region Data is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addRegionRequest.Code)) ModelState.AddModelError(nameof(addRegionRequest.Code), $"{nameof(addRegionRequest.Code)} cannot be null or empty or white space.");

            if (string.IsNullOrWhiteSpace(addRegionRequest.Name)) ModelState.AddModelError(nameof(addRegionRequest.Name), $"{nameof(addRegionRequest.Name)} cannot be null or empty or white space.");

            if (addRegionRequest.Area <= 0) ModelState.AddModelError(nameof(addRegionRequest.Area), $"{nameof(addRegionRequest.Area)} cannot be less then or equal to zero.");

            if (addRegionRequest.Population < 0) ModelState.AddModelError(nameof(addRegionRequest.Population), $"{nameof(addRegionRequest.Population)} cannot be less then zero.");

            if (ModelState.ErrorCount > 0) return false;

            return true;
        }

        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest), "Add Region Data is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code)) ModelState.AddModelError(nameof(updateRegionRequest.Code), $"{nameof(updateRegionRequest.Code)} cannot be null or empty or white space.");
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name)) ModelState.AddModelError(nameof(updateRegionRequest.Name), $"{nameof(updateRegionRequest.Name)} cannot be null or empty or white space.");
            if (updateRegionRequest.Area <= 0) ModelState.AddModelError(nameof(updateRegionRequest.Area), $"{nameof(updateRegionRequest.Area)} cannot be less then or equal to zero.");
            if (updateRegionRequest.Population < 0) ModelState.AddModelError(nameof(updateRegionRequest.Population), $"{nameof(updateRegionRequest.Population)} cannot be less then zero.");
            if (ModelState.ErrorCount > 0) return false;

            return true;
        }

        #endregion
    }
}
