using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Social_network.Utils
{
    public class JwtTokenGenerator
    {
        private readonly string _key;

        public JwtTokenGenerator(IConfiguration config)
        {
            _key = config["Jwt:Key"];
        }

        public string Generate(int userId, string email)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email)
        };

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
