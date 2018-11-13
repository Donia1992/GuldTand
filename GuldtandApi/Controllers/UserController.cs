using System.Threading.Tasks;
using Guldtand.Domain.Helpers;
using Guldtand.Domain.Services;
using Guldtand.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using GuldtandApi.Helpers;

namespace GuldtandApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly ILogger<UserController> _logger;
        private readonly IJWTHelper _jwtHelper;

        public UserController(
            IUserService userService,
            IOptions<AppSettings> appSettings,
            ILogger<UserController> logger,
            IJWTHelper jwtHelper)
        {
            _userService = userService;
            _appSettings = appSettings;
            _logger = logger;
            _jwtHelper = jwtHelper;
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<IActionResult> AuthenticatAsync([FromBody] UserDTO userDto)
        {
            (var user, var roleName) = await _userService.AuthenticateAsync(userDto.Username, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenString = _jwtHelper.GenerateTokenString(user, roleName, _appSettings);

            return Ok(new
            {
                user.Username,
                roleName,
                user.FirstName,
                user.LastName,
                Token = tokenString
            });
        }

        [HttpPost("reg")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDto)
        {
            try
            { 
                await _userService.CreateAsync(userDto, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                _logger.LogError($"Error caught in {nameof(UserController)}, details: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UserDTO userDto)
        {
            
            userDto.Id = id;

            try
            {
                _userService.Update(userDto);
                return Ok();
            }
            catch (AppException ex)
            {
                _logger.LogError($"Error caught in {nameof(UserController)}, details: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }
    }
}