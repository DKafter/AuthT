using AuthExample.Application.Services;
using AuthExample.Core.Abstraction.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthExample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(string username, string email, string password)
        {
            var result = await _userService.CreateUserAsync(username, email, password);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(string email, string password)
        {
            var result = await _userService.AuthenticateUserAsync(email, password);
            return Ok(result);
        }
    }
}
