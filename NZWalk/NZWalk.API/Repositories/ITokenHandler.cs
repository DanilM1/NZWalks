using NZWalk.API.Models.Domain;

namespace NZWalk.API.Repositories
{
    public interface ITokenHandler
    {
        Task<string> CreateTokenAsync(User user);
    }
}
