using NZWalk.API.Models.Domain;

namespace NZWalk.API.Repositories
{
    public interface IUserRepositry
    {
        Task<User> AuthenticateAsync(string username, string password);
    }
}
