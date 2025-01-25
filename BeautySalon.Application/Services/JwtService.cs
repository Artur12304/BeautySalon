namespace BeautySalon.Application.Services
{
    using BeautySalon.Application.Services.Interfaces;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    public class JwtService : IJwtService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly ILogger<JwtService> _logger;

        public JwtService(string key, string issuer, ILogger<JwtService> logger)
        {
            _key = key;
            _issuer = issuer;
            _logger = logger;
        }

        public string GenerateToken(Guid userId, string role)
        {
            _logger.LogInformation("Generating JWT for UserId: {UserId}, Role: {Role}", userId, role);

            try
            {
                var claims = new[]
                {
                    new Claim("UserId", userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _issuer,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                _logger.LogInformation("JWT generated successfully for UserId: {UserId}", userId);
                return tokenString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate JWT for UserId: {UserId}, Role: {Role}", userId, role);
                throw;
            }
        }
    }
}