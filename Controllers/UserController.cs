using Firewall.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Firewall.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{azureAdId}")]
        public async Task<IActionResult> GetByAzureAdId(string azureAdId)
        {
            var user = await _userRepository.GetByAzureAdIdAsync(azureAdId);
            if (user == null)
                return NotFound(new { message = "User not found." });

            return Ok(user);
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
                return BadRequest(new { message = "User data is required." });

            var createdUser = await _userRepository.CreateUserAsync(userDto);
            return CreatedAtAction(nameof(GetByAzureAdId), new { azureAdId = createdUser.azureAdId }, createdUser);
        }
    }
}
