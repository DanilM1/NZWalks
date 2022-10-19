using Microsoft.AspNetCore.Mvc;
using NZWalk.API.Models.DTO;
using NZWalk.API.Repositories;

namespace NZWalk.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepositry userRepositry;
        private readonly ITokenHandler tokenHandler;

        public AuthController(IUserRepositry userRepositry, ITokenHandler tokenHandler)
        {
            this.userRepositry = userRepositry;
            this.tokenHandler = tokenHandler;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {
            // Validate the encoming request
            // Check if user is authenticated
            // Check username and password
            var user = await userRepositry.AuthenticateAsync(loginRequest.Username, loginRequest.Password);

            if (user != null)
            {
                // Generate a JWT Token
                var token = await tokenHandler.CreateTokenAsync(user);
                return Ok(token);
            }

            return BadRequest("Username or Password is incorrect.");
        }
    }
}
