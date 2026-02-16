using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HireTrack.Core.DTOs;
using HireTrack.Core.Models;
using HireTrack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HireTrack.Services
{
    public class AuthService
    {
        private readonly AppDbContext _ctx;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext ctx, IConfiguration config)
        {
            _ctx = ctx;
            _config = config;
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest req)
        {
            var user = await _ctx.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
                return null;

            return new AuthResponse
            {
                Token = GenerateToken(user),
                Username = user.Username,
                Role = user.Role
            };
        }

        public async Task<bool> RegisterAsync(RegisterRequest req)
        {
            if (await _ctx.Users.AnyAsync(u => u.Email == req.Email))
                return false;

            var user = new AppUser
            {
                Username = req.Username,
                Email = req.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
                Role = req.Role
            };

            _ctx.Users.Add(user);
            await _ctx.SaveChangesAsync();
            return true;
        }

        private string GenerateToken(AppUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(double.Parse(_config["Jwt:ExpiresInHours"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
