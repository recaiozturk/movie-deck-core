
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieBringer.Core.Entities;
using MovieBringer.WebApp.Util;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieBringer.Core.Util
{
    public class MyTokenHandler : IMyTokenHandler
    {
        private readonly IConfiguration _config;

        public MyTokenHandler(IConfiguration config)
        {
            _config = config;
        }

        public object GenerateToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Secret").Value ?? "");

            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.UserName ?? "")
                        }
                    ),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "rozturk"
            };

            var token = tokenHandler.CreateToken(tokenDescripter);

            return tokenHandler.WriteToken(token);
        }
    }
}
