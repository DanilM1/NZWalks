using NZWalk.API.Models.Domain;

namespace NZWalk.API.Repositories
{
    public class StaticUserRepositry : IUserRepositry
    {
        private List<User> Users = new List<User>()
        {
            new User()
            {
                FirstName = "Read only",
                LastName = "User",
                EmailAddress = "123@gmail.com",
                Id = Guid.NewGuid(),
                Username = "123@gmail.com",
                Password = "111111111",
                Roles = new List<string> { "reader" }
            },
            new User()
            {
                FirstName = "Read Write",
                LastName = "User",
                EmailAddress = "456@gmail.com",
                Id = Guid.NewGuid(),
                Username = "456@gmail.com",
                Password = "222222222",
                Roles = new List<string> { "reader", "writer" }
            }
        };
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = Users.Find(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) &&
                x.Password == password);

            return user;
        }
    }
}
