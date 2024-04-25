using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Models;
using UsersAPI.ServiceAbstractions;
using UsersAPI.Services.Users;

namespace UsersAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IGetUserByUsernameService _getUserByUsernameService;
        private IJwtService _jwtService;

        public AuthController(IConfiguration configuration, IGetUserByUsernameService getUserByUsernameService, IJwtService jwtService)
        {
            _configuration = configuration;
            _getUserByUsernameService = getUserByUsernameService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login loginModel)
        {
            var user = await _getUserByUsernameService.GetUserByUsername(loginModel.Username);

            if (user == null || user.Password != loginModel.Password)
            {
                return Unauthorized("Credenciales inválidas");
            }
                
            var token = await _jwtService.GenerateJwtToken(user);

            return Ok(new { Token = token });
        }
    }
}
