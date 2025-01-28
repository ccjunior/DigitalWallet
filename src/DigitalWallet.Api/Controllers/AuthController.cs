using DigitalWallet.Api.Configuration;
using DigitalWallet.Domain.Dtos.Request;
using Microsoft.AspNetCore.Mvc;

namespace DigitalWallet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenGeneratorConfiguration _tokenGenerator;

        public AuthController(JwtTokenGeneratorConfiguration tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Validação simulada (substitua por lógica real de validação)
            if (request.Username == "admin" && request.Password == "password")
            {
                var token = _tokenGenerator.GenerateToken(request.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
    }
}

