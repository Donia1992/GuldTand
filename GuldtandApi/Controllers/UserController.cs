using System;
using System.Threading.Tasks;
using Guldtand.Domain.Helpers;
using Guldtand.Domain.Services;
using Guldtand.Domain.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Guldtand.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;

namespace GuldtandApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        [HttpPost]
        [Route("auth")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody]UserDTO userDto)
        {
            var user = await _userService.Authenticate(userDto.Username, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.RoleName)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                user.Username,
                user.Role.RoleName,
                user.FirstName,
                user.LastName,
                Token = tokenString
            });
        }


        [HttpPost]
        [Route("reg")]
        [Authorize(Roles = "Admin")]
        public IActionResult Register([FromBody]UserDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);

            try
            { 
                _userService.Create(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
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

    }
}