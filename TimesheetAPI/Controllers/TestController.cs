namespace TimesheetAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using TimesheetAPI.Data;
    using TimesheetAPI.Models;

    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TestController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("seed")]
        public IActionResult SeedDatabase()
        {
            // Creating a user
            var user = new User { Name = "John Doe", Email = "john@example.com" };
            _context.Users.Add(user);
            _context.SaveChanges();

            // Creating a client
            var client = new Client { Name = "ACME Corp" };
            _context.Clients.Add(client);
            _context.SaveChanges();

            // Assigning the user to the client
            _context.UserClients.Add(new UserClient { UserId = user.Id, ClientId = client.Id });
            _context.SaveChanges();

            // Creating a project for the client
            var project = new Project { Name = "New Website", ClientId = client.Id };
            _context.Projects.Add(project);
            _context.SaveChanges();

            // Creating a task for the project
            var task = new Task { Name = "Design Homepage", ProjectId = project.Id };
            _context.Tasks.Add(task);
            _context.SaveChanges();

            // Creating a timesheet entry
            var timesheet = new TimesheetEntry
            {
                UserId = user.Id,
                TaskId = task.Id,
                Date = DateTime.Now,
                HoursWorked = 5,
                Description = "Worked on homepage design"
            };
            _context.TimesheetEntries.Add(timesheet);
            _context.SaveChanges();

            return Ok("Database seeded successfully!");
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        [HttpGet("projects/{userId}")]
        public IActionResult GetUserProjects(int userId)
        {
            var userProjects = _context.Projects
                .Where(p => p.Client.UserClients.Any(uc => uc.UserId == userId))
                .ToList();

            return Ok(userProjects);
        }
    }

}
