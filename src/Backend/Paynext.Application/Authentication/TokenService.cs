using DotNetEnv;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Paynext.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Paynext.Application.Authentication
{
    public class TokenService(IConfiguration configuration) : ITokenService
    {
        private const int DEFAULT_TOKEN_LIFETIME_MINUTES = 120;
        private readonly IConfiguration _configuration = configuration;

        private string _jwtKey = Environment.GetEnvironmentVariable("TOKEN_JWT_SECRET")?? configuration["Jwt:Key"];
        private string _issuer = Environment.GetEnvironmentVariable("API_URL") ?? configuration["Jwt:Issuer"];
        private string _audience = Environment.GetEnvironmentVariable("APP_URL") ?? configuration["Jwt:Audience"];




        public async Task<AuthenticationResponse> GenerateJwtToken(UserAuthenticateJWT userAuthenticateJwt)
        {
            Env.TraversePath().Load();
            _jwtKey= Environment.GetEnvironmentVariable("TOKEN_JWT_SECRET") ?? _configuration["Jwt:Key"];
            _issuer= Environment.GetEnvironmentVariable("API_URL") ?? _configuration["Jwt:Issuer"];
            _audience = Environment.GetEnvironmentVariable("APP_URL") ?? _configuration["Jwt:Audience"];
            AuthenticationResponse authenticationResponse = new();
            var claims = new ClaimsIdentity(
            [
                new(ClaimTypes.NameIdentifier, userAuthenticateJwt.UserUuid.ToString()),
                new(ClaimTypes.Email, userAuthenticateJwt.UserEmail),
                new(ClaimTypes.Name, userAuthenticateJwt.UserName),
                new(ClaimTypes.Role, userAuthenticateJwt.UserRole),

            ]);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddMinutes(GetTokenLifetimeInMinutes());

            var token = new SecurityTokenDescriptor()
            {
                Issuer = _issuer,
                Audience = _audience,

                Subject = claims,
                Expires = expiry,
                SigningCredentials = creds
            };
            var handler = new JwtSecurityTokenHandler();
            var tokenCreate = handler.CreateToken(token);
            authenticationResponse.AccessToken = handler.WriteToken(tokenCreate);
            authenticationResponse.ExpiresIn = (int)expiry.Subtract(DateTime.UtcNow).TotalMinutes;
            return authenticationResponse;
        }

        public int GetTokenLifetimeInMinutes()
        {
            string tokenLifetime = Environment.GetEnvironmentVariable("TOKEN_JWT_LIFETIME_MINUTES") ?? _configuration["Jwt:TokenLifetimeMinutes"];

            if (int.TryParse(tokenLifetime, out int tokenLifetimeMinutes))
            {
                return tokenLifetimeMinutes;
            }
            return DEFAULT_TOKEN_LIFETIME_MINUTES;
        }
    }
}
