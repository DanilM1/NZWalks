using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalk.API.Models.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public List<string> Roles { get; set; }

        // Navigarion property
        public List<User_Role> Users_Roles { get; set; }
    }
}
