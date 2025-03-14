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
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskDTO taskDTO)
        {
            if (taskDTO == null)
            {
                return BadRequest("Task data is required.");
            }

            var task = new Models.Task
            {
                Name = taskDTO.Name,
                ProjectId = taskDTO.ProjectId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var createdTaskDTO = new TaskDTO
            {
                Id = task.Id,
                Name = task.Name,
                ProjectId = task.ProjectId
            };

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, createdTaskDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound(new { message = $"Task with ID {id} not found." });
            }
            var taskDTO = new TaskDTO
            {
                Id = task.Id,
                Name = task.Name,
                ProjectId = task.ProjectId
            };
            return Ok(taskDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound(new { message = $"Task with ID {id} not found." });
            }
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Task with ID {id} was successfully deleted." });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskDTO updatedTaskDTO)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound(new { message = $"Task with ID {id} not found." });
            }

            // Update Task properties
            task.Name = updatedTaskDTO.Name;
            task.ProjectId = updatedTaskDTO.ProjectId;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Task with ID {id} was successfully updated." });
        }


    }
}
