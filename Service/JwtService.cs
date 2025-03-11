using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ContactManagementApi.OutputDirectory;
using Microsoft.IdentityModel.Tokens;

namespace ContactManagementApi.Service
{

    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("jwt is not configured");
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roleClaims=user.Roles.Select(r => new Claim(ClaimTypes.Role,r.RoleName)).ToList();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                
            };
            claims.AddRange(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

           return new JwtSecurityTokenHandler().WriteToken(token);
       }
    }
}