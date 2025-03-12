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
    public class TimesheetEntryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TimesheetEntryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTimesheetEntry([FromBody] TimesheetEntryDTO TimesheetEntryDTO)
        {
            if (TimesheetEntryDTO == null)
            {
                return BadRequest("TimesheetEntry data is required.");
            }

            var TimesheetEntry = new TimesheetEntry
            {
                UserId = TimesheetEntryDTO.UserId,
                TaskId = TimesheetEntryDTO.TaskId,
                Date = TimesheetEntryDTO.Date,
                HoursWorked = TimesheetEntryDTO.HoursWorked,
                Description = TimesheetEntryDTO.Description
            };

            _context.TimesheetEntries.Add(TimesheetEntry);
            await _context.SaveChangesAsync();

            // ✅ Return the actual saved entity with the correct ID
            var createdTimesheetEntryDTO = new TimesheetEntryDTO
            {
                Id = TimesheetEntry.Id,
                UserId = TimesheetEntry.UserId,
                TaskId= TimesheetEntry.TaskId,
                Date = TimesheetEntry.Date,
                HoursWorked = TimesheetEntry.HoursWorked,
                Description = TimesheetEntry.Description
            };

            return CreatedAtAction(nameof(GetTimesheetEntryById), new { id = createdTimesheetEntryDTO.Id }, createdTimesheetEntryDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTimesheetEntryById(int id)
        {
            var TimesheetEntry = await _context.TimesheetEntries.FindAsync(id);
            if (TimesheetEntry == null)
            {
                return NotFound(new { message = $"TimesheetEntry with ID {id} not found." });
            }

            var TimesheetEntryDTO = new TimesheetEntryDTO
            {
                Id = TimesheetEntry.Id,
                UserId = TimesheetEntry.UserId,
                TaskId = TimesheetEntry.TaskId,
                Date = TimesheetEntry.Date,
                HoursWorked = TimesheetEntry.HoursWorked,
                Description = TimesheetEntry.Description
            };

            return Ok(TimesheetEntryDTO);
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
        public async Task<IActionResult> UpdateTimesheetEntry(int id, [FromBody] TimesheetEntryDTO updatedTimesheetEntryDTO)
        {
            var TimesheetEntry = await _context.TimesheetEntries.FindAsync(id);

            if (TimesheetEntry == null)
            {
                return NotFound(new { message = $"TimesheetEntry with ID {id} not found." });
            }

            // Update TimesheetEntry properties
            TimesheetEntry.Date = updatedTimesheetEntryDTO.Date;
            TimesheetEntry.HoursWorked = updatedTimesheetEntryDTO.HoursWorked;
            TimesheetEntry.Description = updatedTimesheetEntryDTO.Description;

            _context.TimesheetEntries.Update(TimesheetEntry);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"TimesheetEntry with ID {id} was successfully updated." });
        }


    }
}
