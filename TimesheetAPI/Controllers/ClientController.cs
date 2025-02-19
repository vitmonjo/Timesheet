using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimesheetAPI.Data;
using TimesheetAPI.Models;
using System.Threading.Tasks;

namespace TimesheetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] Client client)
        {
            if (client == null)
            {
                return BadRequest("Client data is required.");
            }

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClientById), new { id = client.Id }, client);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var Client = await _context.Clients.FindAsync(id);
            if (Client == null)
            {
                return NotFound(new { message = $"Client with ID {id} not found." });
            }
            return Ok(Client);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var Client = await _context.Clients.FindAsync(id);
            if (Client == null)
            {
                return NotFound(new { message = $"Client with ID {id} not found." });
            }
            _context.Clients.Remove(Client);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Client with ID {id} was successfully deleted." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] Client updatedClient)
        {
            var Client = await _context.Clients.FindAsync(id);

            if (Client == null)
            {
                return NotFound(new { message = $"Client with ID {id} not found." });
            }

            // Update Client properties
            Client.Name = updatedClient.Name;

            _context.Clients.Update(Client);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Client with ID {id} was successfully updated." });
        }

    }
}
