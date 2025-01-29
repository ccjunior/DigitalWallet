using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DigitalWallet.Api.Configuration
{
    public class JwtTokenGeneratorConfiguration
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGeneratorConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Guid userId, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Claims do token
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()), // ID do usuário
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // ID único do token
            new Claim(ClaimTypes.Role, role), // Role do usuário
            new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]), // Emissor
            new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]) // Audiência
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Expiração de 1 hora
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //private readonly IConfiguration _configuration;

        //public JwtTokenGeneratorConfiguration(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        //public string GenerateToken(Guid userId)
        //{
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    };

        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["Jwt:Issuer"],
        //        audience: _configuration["Jwt:Audience"],
        //        claims: claims,
        //        expires: DateTime.Now.AddHours(1),
        //        signingCredentials: creds);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}
