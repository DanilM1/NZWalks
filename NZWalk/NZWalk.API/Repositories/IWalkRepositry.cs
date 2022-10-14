using NZWalk.API.Models.Domain;

namespace NZWalk.API.Repositories
{
    public interface IWalkRepositry
    {
        Task<IEnumerable<Walk>> GetAllAsync();
        Task<Walk> GetAsync(Guid id);
        Task<Walk> AddAsync(Walk walk);
        Task<Walk> DeleteAsync(Guid id);
        Task<Walk> UpdateAsync(Guid id, Walk walk);
    }
}
