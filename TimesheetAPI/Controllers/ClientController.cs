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
    public class ClientController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] ClientDTO clientDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = new Client
            {
                Name = clientDTO.Name
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            var createdClientDTO = new ClientDTO
            {
                Id = client.Id,
                Name = client.Name
            };

            return CreatedAtAction(nameof(GetClientById), new { id = client.Id }, createdClientDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var Client = await _context.Clients.FindAsync(id);
            if (Client == null)
            {
                return NotFound(new { message = $"Client with ID {id} not found." });
            }
            var clientDTO = new ClientDTO
            {
                Name = Client.Name
            };
            return Ok(clientDTO);
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] ProjectDTO updatedClientDTO)
        {
            var Client = await _context.Clients.FindAsync(id);

            if (Client == null)
            {
                return NotFound(new { message = $"Client with ID {id} not found." });
            }

            Client.Name = updatedClientDTO.Name;

            _context.Clients.Update(Client);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Client with ID {id} was successfully updated." });
        }

    }
}
