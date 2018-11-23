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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly ILogger<UsersController> _logger;
        private readonly IJWTHelper _jwtHelper;

        public UsersController(
            IUserService userService,
            IOptions<AppSettings> appSettings,
            ILogger<UsersController> logger,
            IJWTHelper jwtHelper)
        {
            _userService = userService;
            _appSettings = appSettings;
            _logger = logger;
            _jwtHelper = jwtHelper;
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] UserDTO userDto)
        {
            (var user, var roleName) = _userService.Authenticate(userDto.Username, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var token = _jwtHelper.GenerateTokenString(user, roleName, _appSettings);
            Request.HttpContext.Response.Headers.Add("X-Guldtand-Token", token);

            return Ok(new
            {
                user.Username,
                roleName,
                user.FirstName,
                user.LastName,
            });
        }

        [HttpPost]
        public IActionResult Register([FromBody] UserDTO userDto)
        {
            try
            { 
                _userService.Create(userDto, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                _logger.LogError($"Error caught in {nameof(UsersController)}, details: {ex.Message}");
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
                _logger.LogError($"Error caught in {nameof(UsersController)}, details: {ex.Message}");
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