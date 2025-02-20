using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimesheetAPI.Data;
using TimesheetAPI.Models;
using System.Threading.Tasks;

namespace TimesheetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetEntryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TimesheetEntryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTimesheetEntry([FromBody] TimesheetEntry TimesheetEntry)
        {
            if (TimesheetEntry == null)
            {
                return BadRequest("TimesheetEntry data is required.");
            }

            _context.TimesheetEntries.Add(TimesheetEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTimesheetEntryById), new { id = TimesheetEntry.Id }, TimesheetEntry);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTimesheetEntryById(int id)
        {
            var TimesheetEntry = await _context.TimesheetEntries.FindAsync(id);
            if (TimesheetEntry == null)
            {
                return NotFound(new { message = $"TimesheetEntry with ID {id} not found." });
            }
            return Ok(TimesheetEntry);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimesheetEntry(int id)
        {
            var TimesheetEntry = await _context.TimesheetEntries.FindAsync(id);
            if (TimesheetEntry == null)
            {
                return NotFound(new { message = $"TimesheetEntry with ID {id} not found." });
            }
            _context.TimesheetEntries.Remove(TimesheetEntry);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"TimesheetEntry with ID {id} was successfully deleted." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTimesheetEntry(int id, [FromBody] TimesheetEntry updatedTimesheetEntry)
        {
            var TimesheetEntry = await _context.TimesheetEntries.FindAsync(id);

            if (TimesheetEntry == null)
            {
                return NotFound(new { message = $"TimesheetEntry with ID {id} not found." });
            }

            // Update TimesheetEntry properties
            TimesheetEntry.Date = updatedTimesheetEntry.Date;
            TimesheetEntry.HoursWorked = updatedTimesheetEntry.HoursWorked;
            TimesheetEntry.Description = updatedTimesheetEntry.Description;

            _context.TimesheetEntries.Update(TimesheetEntry);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"TimesheetEntry with ID {id} was successfully updated." });
        }

    }
}
