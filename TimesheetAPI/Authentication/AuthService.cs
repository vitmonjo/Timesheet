using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using TimesheetAPI.DTOs;
using TimesheetAPI.Models;

namespace TimesheetAPI.Authentication
{
    public class AuthService
    {
        private readonly Dictionary<string, User> _users = new();  // Temporary in-memory user storage
        private readonly string _jwtSecret;

        public AuthService(IConfiguration configuration)
        {
            _jwtSecret = configuration["Jwt:Secret"]; // Load secret key from appsettings.json
        }

        public AuthResponseDTO Register(RegisterDTO registerDto)
        {
            if (_users.ContainsKey(registerDto.Email))
                throw new Exception("User already exists");

            // Hash the password before storing
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Create and store user
            var user = new User { Email = registerDto.Email, PasswordHash = hashedPassword };
            _users[user.Email] = user;

            // Generate JWT token for the new user
            string token = GenerateJwtToken(user);

            return new AuthResponseDTO { Token = token };
        }

        public AuthResponseDTO Login(LoginDTO loginDto)
        {
            if (!_users.TryGetValue(loginDto.Email, out var user))
                throw new Exception("Invalid email or password");

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                throw new Exception("Invalid email or password");

            // Generate JWT token
            string token = GenerateJwtToken(user);

            return new AuthResponseDTO { Token = token };
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
