using Microsoft.IdentityModel.Tokens;
using MsEmail.Domain.Entities;
using MSEmail.Common.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MSEmail.Infra.Services
{
    public static class TokenServices 
    {
        public static string GenerateToken(User user) 
        {
            var handler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(ConfigHelper.GetTokenSecret());

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(8)
            };

            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateClaims(User user)
        {
            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            ci.AddClaim(new Claim(ClaimTypes.Role, user.Role));
            ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            return ci;
        }
    }
}
