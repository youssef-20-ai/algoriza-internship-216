using AlgorizaProject.DAL.Entities;
using APIDemo.BLL.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace APIDemo.BLL.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateToken(AppUser user, UserManager<AppUser> userManager)
        {
            var authClaim = new List<Claim>() 
            {
                new Claim(ClaimTypes.Email, user.Email),
            };

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
                authClaim.Add(new Claim(ClaimTypes.Role, role.ToString()));

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInMinutes"])),
                claims: authClaim,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)                
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
