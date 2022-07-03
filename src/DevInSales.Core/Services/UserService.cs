using DevInSales.Core.Data.Context;
using DevInSales.Core.Data.Dtos;
using DevInSales.EFCoreApi.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DevInSales.Core.Entities
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private User _user;
        public UserService(DataContext context,
                           UserManager<User> userManager,
                           IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var token = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var expiration = DateTime.Now.AddSeconds(Convert.ToDouble(jwtSettings.GetSection("lifetime").Value));

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
                );

            return token;
        }
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, _user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, _user.Email));
            
            var roles = await _userManager.GetRolesAsync(_user);

            if (!roles.Any())
                await _userManager.AddToRoleAsync(_user, "User");

            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            return claims;
        }
        private SigningCredentials GetSigningCredentials()
        {
            var key = _configuration.GetSection("Jwt:key").Value;
            var secret = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<bool> ValidateUser(UserLoginRequest model)
        {
            _user = await _userManager.FindByNameAsync(model.Email);
            var validPassword = await _userManager.CheckPasswordAsync(_user, model.Senha);
            return (_user is not null && validPassword);
        }

        public async Task<User?> ObterPorId(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<List<User>> ObterUsers(string? name, string? DataMin, string? DataMax)
        {
            var query = _context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Nome.ToUpper().Contains(name.ToUpper()));
            if (!string.IsNullOrEmpty(DataMin))
                query = query.Where(p => p.DataNascimento >= DateTime.Parse(DataMin));
            if (!string.IsNullOrEmpty(DataMax))
                query = query.Where(p => p.DataNascimento <= DateTime.Parse(DataMax));

            return await query.ToListAsync();
        }
        public async Task<IdentityResult> RemoverUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return await _userManager.DeleteAsync(user);
        }

    }
}