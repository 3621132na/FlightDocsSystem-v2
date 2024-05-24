using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.Models;
using UserService.Services;

namespace UserManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user, string password)
        {
            try
            {
                var registeredUser = await _userService.RegisterUserAsync(user, password);
                return Ok(registeredUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var user = await _userService.LoginAsync(username, password);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok(new { message = "Logout successful." });
        }
        [HttpGet("userinfo")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();
                var user = await _userService.GetUserByIdAsync(int.Parse(userId));
                if (user == null)
                    return NotFound(new { message = "User not found." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
