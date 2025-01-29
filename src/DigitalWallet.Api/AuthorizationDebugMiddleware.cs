using System.IdentityModel.Tokens.Jwt;

namespace DigitalWallet.Api
{
    public class AuthorizationDebugMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthorizationDebugMiddleware> _logger;

        public AuthorizationDebugMiddleware(RequestDelegate next, ILogger<AuthorizationDebugMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            _logger.LogInformation("=== Authorization Debug Info ===");

            if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                _logger.LogInformation($"Authorization Header: {authHeader}");

                // Decodificar o token para debug
                try
                {
                    var token = authHeader.ToString().Replace("Bearer ", "");
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);

                    _logger.LogInformation("Token Details:");
                    _logger.LogInformation($"Issuer: {jwtToken.Issuer}");
                    _logger.LogInformation($"Audience: {jwtToken.Audiences.FirstOrDefault()}");
                    _logger.LogInformation($"Expiration: {jwtToken.ValidTo}");

                    foreach (var claim in jwtToken.Claims)
                    {
                        _logger.LogInformation($"Claim: {claim.Type} = {claim.Value}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error decoding token: {ex.Message}");
                }
            }

            await _next(context);
        }
    }

    public static class AuthorizationDebugMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthorizationDebug(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationDebugMiddleware>();
        }
    }
}
