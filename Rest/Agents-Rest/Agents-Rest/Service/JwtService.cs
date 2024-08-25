using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Agents_Rest.Service
{
    public class JwtService(IConfiguration configuration) : IJwtService
    {
        public string GenerateToken(string uniqueIdentifier)
        {
            string key = configuration.GetValue("Jwt:Key", string.Empty)
                ?? throw new ArgumentNullException("Key does not exists on Jwt");

            int expiry = configuration.GetValue("Jwt:ExpiryInMinutes", 60);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentils = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "SimulationServer"),
                new Claim(ClaimTypes.NameIdentifier, "MVCServer")
            };

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(expiry),
                claims: claims,
                signingCredentials: credentils
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
