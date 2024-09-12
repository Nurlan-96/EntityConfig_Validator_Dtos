using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopAppAPI.Entities;
using ShopAppAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopAppAPI.Services.Implementations
{

    public class TokenService : ITokenService
    {
        public string GetToken(string secretKey,string audience, string issuer, AppUser user, IList<string>roles)
        {
            var handler = new JwtSecurityTokenHandler();
            var privateKey = Encoding.UTF8.GetBytes(secretKey);
            var credentials = new SigningCredentials(
              new SymmetricSecurityKey(privateKey),
              SecurityAlgorithms.HmacSha256);

            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim("id", user.Id));
            ci.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            ci.AddClaim(new Claim(ClaimTypes.GivenName, user.FullName));
            ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            ci.AddClaims(roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList());

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(1),
                Subject = ci,
                Audience = audience,
                Issuer = issuer,
            };
            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }
    }
}
