using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalk.API.Models.DTO;
using NZWalk.API.Repositories;

namespace NZWalk.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepositry walkDifficultyRepositry;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepositry walkDifficultyRepositry, IMapper mapper)
        {
            this.walkDifficultyRepositry = walkDifficultyRepositry;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiesAsync()
        {
            var walkDifficulties = await walkDifficultyRepositry.GetAllAsync();

            if (walkDifficulties == null) return NotFound();

            var walkDifficultiesDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulties);

            return Ok(walkDifficultiesDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepositry.GetAsync(id);

            if (walkDifficulty == null) return NotFound();

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            // Validate The Request
            if (!ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest)) return BadRequest(ModelState);

            // Request(DTO) to Domain model
            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code
            };

            // Pass details to Repository
            walkDifficulty = await walkDifficultyRepositry.AddAsync(walkDifficulty);

            // Convert back to DTO
            var walkDifficultyDTO = new Models.DTO.WalkDifficulty
            {
                Id = walkDifficulty.Id,
                Code = walkDifficulty.Code
            };

            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepositry.DeleteAsync(id);

            if (walkDifficulty == null) return NotFound();

            return Ok(mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty));
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            // Validate The Request
            if (!ValidateUpdateWalkDifficultyAsync(updateWalkDifficultyRequest)) return BadRequest(ModelState);

            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code
            };

            walkDifficulty = await walkDifficultyRepositry.UpdateAsync(id, walkDifficulty);

            if (walkDifficulty == null) return NotFound();

            var walkDifficultyDTO = new Models.DTO.WalkDifficulty
            {
                Id = walkDifficulty.Id,
                Code = walkDifficulty.Code
            };

            return Ok(walkDifficultyDTO);
        }

        #region Private methods

        private bool ValidateAddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if (addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest), "Add Walk Difficulty Data is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code)) ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code), $"{nameof(addWalkDifficultyRequest.Code)} cannot be null or empty or white space.");

            if (ModelState.ErrorCount > 0) return false;

            return true;
        }

        private bool ValidateUpdateWalkDifficultyAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest), "Add Region Data is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code)) ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code), $"{nameof(updateWalkDifficultyRequest.Code)} cannot be null or empty or white space.");
            if (ModelState.ErrorCount > 0) return false;

            return true;
        }

        #endregion
    }
}