using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimesheetAPI.Data;
using TimesheetAPI.Models;
using System.Threading.Tasks;
using TimesheetAPI.DTOs;

namespace TimesheetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest("User data is required.");
            }

            // Hash the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);

            var user = new User
            {
                Email = userDTO.Email,
                Name = userDTO.Name,
                PasswordHash = passwordHash
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var createdUserDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
                // Don't include Password in the response
            };

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, createdUserDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            var userDTO = new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name
            };

            return Ok(userDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"User with ID {id} was successfully deleted." });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO updatedUserDTO)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            // Update user properties
            user.Email = updatedUserDTO.Email;
            user.Name = updatedUserDTO.Name;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"User with ID {id} was successfully updated." });
        }

    }
}
