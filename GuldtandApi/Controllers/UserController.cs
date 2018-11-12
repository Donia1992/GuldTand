using System.Threading.Tasks;
using Guldtand.Domain.Helpers;
using Guldtand.Domain.Services;
using Guldtand.Domain.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.Extensions.Options;
using Guldtand.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using GuldtandApi.Helpers;

namespace GuldtandApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly ILogger<UserController> _logger;
        private readonly IJWTHelper _jwtHelper;

        public UserController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            ILogger<UserController> logger,
            IJWTHelper jwtHelper)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings;
            _logger = logger;
            _jwtHelper = jwtHelper;
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<IActionResult> Authenticate([FromBody]UserDTO userDto)
        {
            var user = await _userService.Authenticate(userDto.Username, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenString = _jwtHelper.GenerateTokenString(user, _appSettings);

            return Ok(new
            {
                user.Username,
                user.Role.RoleName,
                user.FirstName,
                user.LastName,
                Token = tokenString
            });
        }

        [HttpPost("reg")]
        public IActionResult Register([FromBody] UserDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);

            try
            { 
                _userService.Create(user, userDto.Password);
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
            var userDtos = _mapper.Map<IList<UserDTO>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var userDto = _mapper.Map<UserDTO>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UserDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Id = id;

            try
            {
                _userService.Update(user, userDto.Password);
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