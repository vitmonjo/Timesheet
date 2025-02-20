using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimesheetAPI.Data;
using TimesheetAPI.Models;
using System.Threading.Tasks;

namespace TimesheetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] Models.Task Task)
        {
            if (Task == null)
            {
                return BadRequest("Task data is required.");
            }

            _context.Tasks.Add(Task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTaskById), new { id = Task.Id }, Task);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var Task = await _context.Tasks.FindAsync(id);
            if (Task == null)
            {
                return NotFound(new { message = $"Task with ID {id} not found." });
            }
            return Ok(Task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var Task = await _context.Tasks.FindAsync(id);
            if (Task == null)
            {
                return NotFound(new { message = $"Task with ID {id} not found." });
            }
            _context.Tasks.Remove(Task);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Task with ID {id} was successfully deleted." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] Models.Task updatedTask)
        {
            var Task = await _context.Tasks.FindAsync(id);

            if (Task == null)
            {
                return NotFound(new { message = $"Task with ID {id} not found." });
            }

            // Update Task properties
            Task.Name = updatedTask.Name;
            Task.ProjectId = updatedTask.ProjectId;

            _context.Tasks.Update(Task);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Task with ID {id} was successfully updated." });
        }

    }
}
