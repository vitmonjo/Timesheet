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
    public class UserClientController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserClientController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserClient([FromBody] UserClientDTO UserClientDTO)
        {
            if (UserClientDTO == null)
            {
                return BadRequest("UserClient data is required.");
            }

            var UserClient = new UserClient
            {
                UserId = UserClientDTO.UserId,
                ClientId = UserClientDTO.ClientId
            };

            _context.UserClients.Add(UserClient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserClientById), new { id = UserClient.Id }, UserClientDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserClientById(int id)
        {
            var UserClient = await _context.UserClients.FindAsync(id);
            if (UserClient == null)
            {
                return NotFound(new { message = $"UserClient with ID {id} not found." });
            }

            var UserClientDTO = new UserClientDTO
            {
                Id = UserClient.Id,
                UserId = UserClient.UserId,
                ClientId = UserClient.ClientId
            };

            return Ok(UserClientDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserClient(int id)
        {
            var UserClient = await _context.UserClients.FindAsync(id);
            if (UserClient == null)
            {
                return NotFound(new { message = $"UserClient with ID {id} not found." });
            }
            _context.UserClients.Remove(UserClient);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"UserClient with ID {id} was successfully deleted." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserClient(int id, [FromBody] UserClientDTO updatedUserClientDTO)
        {
            var UserClient = await _context.UserClients.FindAsync(id);

            if (UserClient == null)
            {
                return NotFound(new { message = $"UserClient with ID {id} not found." });
            }

            // Update UserClient properties
            UserClient.UserId = updatedUserClientDTO.UserId;
            UserClient.ClientId = updatedUserClientDTO.ClientId;

            _context.UserClients.Update(UserClient);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"UserClient with ID {id} was successfully updated." });
        }


    }
}
