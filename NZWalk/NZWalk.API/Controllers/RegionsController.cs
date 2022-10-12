using AutoMapper;
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
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await regionRepositry.GetAllAsync();

            // return DTO regions
            // var regionsDTO = new List<Models.DTO.Region>();
            // regions.ToList().ForEach(region =>
            // {
            // var regionDTO = new Models.DTO.Region()
            // {
            // Id = region.Id,
            // Code = region.Code,
            // Name = region.Name,
            // Area = region.Area,
            // Lat = region.Lat,
            // Long = region.Long,
            // Population = region.Population
            // };
            // regionsDTO.Add(regionDTO);
            // });

            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);
        }
    }
}
