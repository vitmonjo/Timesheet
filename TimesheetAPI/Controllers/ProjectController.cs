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
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectDTO projectDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = new Project
            {
                Name = projectDTO.Name,
                ClientId = projectDTO.ClientId
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var createdProjectDTO = new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name
            };

            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, createdProjectDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound(new { message = $"Project with ID {id} not found." });
            }
            var projectDTO = new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name
            };
            return Ok(projectDTO);
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
        public async Task<IActionResult> UpdateProject(int id, [FromBody] ProjectDTO updatedProjectDTO)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound(new { message = $"Project with ID {id} not found." });
            }

            project.Name = updatedProjectDTO.Name;
            project.ClientId = updatedProjectDTO.ClientId;

            _context.Projects.Update(project);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Project with ID {id} was successfully updated." });
        }


    }
}
