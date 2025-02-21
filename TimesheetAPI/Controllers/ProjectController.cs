using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimesheetAPI.Data;
using TimesheetAPI.Models;
using System.Threading.Tasks;

namespace TimesheetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var Project = await _context.Projects.FindAsync(id);
            if (Project == null)
            {
                return NotFound(new { message = $"Project with ID {id} not found." });
            }
            return Ok(Project);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var Project = await _context.Projects.FindAsync(id);
            if (Project == null)
            {
                return NotFound(new { message = $"Project with ID {id} not found." });
            }
            _context.Projects.Remove(Project);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Project with ID {id} was successfully deleted." });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] Project updatedProject)
        {
            var Project = await _context.Projects.FindAsync(id);

            if (Project == null)
            {
                return NotFound(new { message = $"Project with ID {id} not found." });
            }

            // Update Project properties
            Project.Name = updatedProject.Name;
            Project.ClientId = updatedProject.ClientId;

            _context.Projects.Update(Project);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Project with ID {id} was successfully updated." });
        }

    }
}
