using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Translation.API.Models.Configurations;
using Translation.API.Models.Responses;

namespace Translation.API.Services
{
    public class AuthService : IAuthService
    {
        private JwtConfig JwtConfig { get; }
        public AuthService(IOptions<JwtConfig> options)
        {
            JwtConfig = options.Value;
        }
        public ServiceResponse<TokenResponse> GenerateJwtToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(JwtConfig.SeceretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", Guid.NewGuid().ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(90),
                Audience = JwtConfig.Audience,
                Issuer = JwtConfig.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var serviceResponse = new ServiceResponse<TokenResponse>
            {
                Data = new TokenResponse
                {
                    Token = tokenHandler.WriteToken(token)
                },
            };
            return serviceResponse;
        }
    }
}
