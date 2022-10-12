using NZWalk.API.Models.Domain;

namespace NZWalk.API.Repositories
{
    public interface IRegionRepositry
    {
        Task<IEnumerable<Region>> GetAllAsync();
    }
}
