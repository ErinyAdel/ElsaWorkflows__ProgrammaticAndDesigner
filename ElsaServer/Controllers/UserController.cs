using ElsaServer.Interfaces;
using ElsaServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElsaServer.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("CreateUserAsync")]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserModel user)
        {
            var res = await _userService.CreateUserAsync(user);
            
            return res == 1 ? Ok("User created successfully.") : BadRequest("User failed to be created.");
        }
    }
}
