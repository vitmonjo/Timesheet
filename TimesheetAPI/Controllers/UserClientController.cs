using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimesheetAPI.Data;
using TimesheetAPI.Models;
using System.Threading.Tasks;

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
        public async Task<IActionResult> CreateUserClient([FromBody] UserClient UserClient)
        {
            if (UserClient == null)
            {
                return BadRequest("UserClient data is required.");
            }

            _context.UserClients.Add(UserClient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserClientById), new { id = UserClient.Id }, UserClient);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserClientById(int id)
        {
            var UserClient = await _context.UserClients.FindAsync(id);
            if (UserClient == null)
            {
                return NotFound(new { message = $"UserClient with ID {id} not found." });
            }
            return Ok(UserClient);
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
        public async Task<IActionResult> UpdateUserClient(int id, [FromBody] UserClient updatedUserClient)
        {
            var UserClient = await _context.UserClients.FindAsync(id);

            if (UserClient == null)
            {
                return NotFound(new { message = $"UserClient with ID {id} not found." });
            }

            // Update UserClient properties
            UserClient.UserId = updatedUserClient.UserId;
            UserClient.ClientId = updatedUserClient.ClientId;

            _context.UserClients.Update(UserClient);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"UserClient with ID {id} was successfully updated." });
        }

    }
}
