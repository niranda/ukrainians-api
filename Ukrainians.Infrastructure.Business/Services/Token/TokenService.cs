using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ukrainians.Infrastrusture.Data.Entities;
using Ukrainians.UtilityServices.Settings;

namespace Ukrainians.Infrastructure.Business.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public string GenerateJWT(User user)
        {
            var identity = GetIdentity(user);

            var timeNow = DateTime.UtcNow;
            var secret = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor();
            var tokenHandler = new JwtSecurityTokenHandler();

            tokenDescriptor.NotBefore = timeNow;
            tokenDescriptor.Subject = new ClaimsIdentity(identity.Claims);
            tokenDescriptor.SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);


            tokenDescriptor.Expires = timeNow.AddDays(1);

            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            var encodedJwt = tokenHandler.WriteToken(jwt);

            return encodedJwt;
        }

        private ClaimsIdentity GetIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("name", user.UserName),
                new Claim("nameToDisplay", user.NameToDisplay),
                new Claim("role", user.Role.Name),
                new Claim("email", user.Email),
                new Claim("id", user.Id.ToString())
            };

            return new ClaimsIdentity(claims, "Token", "name", "role");
        }
    }
}
