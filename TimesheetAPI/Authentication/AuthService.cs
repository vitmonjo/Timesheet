using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TimesheetAPI.Data;
using TimesheetAPI.DTOs;
using TimesheetAPI.Models;

namespace TimesheetAPI.Authentication
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly string? _jwtSecret;

        public AuthService(IConfiguration configuration, AppDbContext context)
        {
            _jwtSecret = configuration["Jwt:Secret"]; // Load secret key from appsettings.json
            _context = context;
        }

        public async Task<AuthResponseDTO> Register(RegisterDTO registerDto)
        {
            // Check if user already exists in database
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                throw new Exception("User already exists");

            // Validate password strength
            ValidatePassword(registerDto.Password);

            // Hash the password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Create user
            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                PasswordHash = hashedPassword,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generate JWT
            string token = GenerateJwtToken(user);
            return new AuthResponseDTO { Token = token };
        }


        public async Task<AuthResponseDTO> Login(LoginDTO loginDto)
        {
            // Find user by email in the database
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
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

        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required.");

            var regex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{10,}$");

            if (!regex.IsMatch(password))
                throw new Exception("Password must be at least 10 characters long and contain at least one uppercase letter, one number, and one special character.");
        }
    }
}
