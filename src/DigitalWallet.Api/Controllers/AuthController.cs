using DigitalWallet.Api.Configuration;
using DigitalWallet.Api.Provider;
using DigitalWallet.Application.Services;
using DigitalWallet.Domain.Common;
using DigitalWallet.Domain.Dtos.Request;
using Microsoft.AspNetCore.Mvc;

namespace DigitalWallet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        //private readonly JwtTokenGeneratorConfiguration _tokenGenerator;
        private readonly TokenProvider _tokenProvider;
        private readonly IUserService _userService;

        public AuthController(IUserService userService, TokenProvider tokenProvider)
        {
            _userService = userService;
            _tokenProvider = tokenProvider;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Email e senha são obrigatórios.");

            var user = await _userService.GetUserByEmailAsync(request.Email);

            if (user.Data.Email == request.Email && user.Data.PasswordHash == PasswordHasher.HashPassword(request.Password))
            {
                var token = _tokenProvider.Create(user.Data.UserId.ToString(), user.Data.Email);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
    }
}

