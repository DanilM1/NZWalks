using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalk.API.Repositories;

namespace NZWalk.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepositry walkRepositry;
        private readonly IMapper mapper;
        private readonly IRegionRepositry regionRepositry;
        private readonly IWalkDifficultyRepositry walkDifficultyRepositry;

        public WalksController(IWalkRepositry walkRepositry, IMapper mapper, IRegionRepositry regionRepositry, IWalkDifficultyRepositry walkDifficultyRepositry)
        {
            this.walkRepositry = walkRepositry;
            this.mapper = mapper;
            this.regionRepositry = regionRepositry;
            this.walkDifficultyRepositry = walkDifficultyRepositry;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var walks = await walkRepositry.GetAllAsync();

            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walks);

            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walk = await walkRepositry.GetAsync(id);

            if (walk == null) return NotFound();

            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            // Validate The Request
            if (!await ValidateAddWalkAsync(addWalkRequest)) return BadRequest(ModelState);

            // Request(DTO) to Domain model
            var walk = new Models.Domain.Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            // Pass details to Repository
            walk = await walkRepositry.AddAsync(walk);

            // Convert back to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walk.Id,
                Name = walk.Name,
                Length = walk.Length,
                RegionId = walk.RegionId,
                WalkDifficultyId = walk.WalkDifficultyId
            };

            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var walk = await walkRepositry.DeleteAsync(id);

            if (walk == null) return NotFound();

            return Ok(mapper.Map<Models.DTO.Walk>(walk));
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            // Validate The Request
            if (!await ValidateUpdateWalkAsync(updateWalkRequest)) return BadRequest(ModelState);

            var walk = new Models.Domain.Walk()
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            walk = await walkRepositry.UpdateAsync(id, walk);

            if (walk == null) return NotFound();

            var walkDTO = new Models.DTO.Walk
            {
                Id = walk.Id,
                Name = walk.Name,
                Length = walk.Length,
                RegionId = walk.RegionId,
                WalkDifficultyId = walk.WalkDifficultyId
            };

            return Ok(walkDTO);
        }

        #region Private methods
        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), $"{nameof(addWalkRequest)} cannot be empty.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addWalkRequest.Name)) ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} cannot be null or empty or white space.");
            if (addWalkRequest.Length <= 0) ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} cannot be less then zero.");

            var region = await regionRepositry.GetAsync(addWalkRequest.RegionId);
            if (region == null) ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is invalid.");

            var walkDifficulty = await walkDifficultyRepositry.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null) ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid.");

            if (ModelState.ErrorCount > 0) return false;

            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest), $"{nameof(updateWalkRequest)} cannot be empty.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateWalkRequest.Name)) ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} cannot be null or empty or white space.");
            if (updateWalkRequest.Length <= 0) ModelState.AddModelError(nameof(updateWalkRequest.Length), $"{nameof(updateWalkRequest.Length)} cannot be less then zero.");

            var region = await regionRepositry.GetAsync(updateWalkRequest.RegionId);
            if (region == null) ModelState.AddModelError(nameof(updateWalkRequest.RegionId), $"{nameof(updateWalkRequest.RegionId)} is invalid.");

            var walkDifficulty = await walkDifficultyRepositry.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null) ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId), $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid.");

            if (ModelState.ErrorCount > 0) return false;

            return true;
        }
        #endregion
    }
}
