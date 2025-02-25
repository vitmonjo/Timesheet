using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimesheetAPI.Data;
using TimesheetAPI.Models;
using Task = TimesheetAPI.Models.Task;

namespace TimesheetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly Random _random = new();

        public SeedController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("populate")]
        public async Task<IActionResult> PopulateDatabase()
        {
            if (await _context.Clients.AnyAsync())
            {
                return BadRequest("Database already contains data. Clear it first.");
            }

            var clients = new List<Client>();
            var projects = new List<Project>();
            var tasks = new List<Task>();
            var timesheetEntries = new List<TimesheetEntry>();
            var users = new List<User>();
            var userClients = new List<UserClient>();

            // Create 10 Clients
            for (int i = 1; i <= 10; i++)
            {
                var client = new Client { Name = $"Client {i}" };
                clients.Add(client);
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                // Create 3 Projects per Client
                for (int j = 1; j <= 3; j++)
                {
                    var project = new Project { Name = $"Project {j} for Client {i}", ClientId = client.Id };
                    projects.Add(project);
                    _context.Projects.Add(project);
                    await _context.SaveChangesAsync();

                    // Create 10 Tasks per Project
                    for (int k = 1; k <= 10; k++)
                    {
                        var task = new Task { Name = $"Task {k} for Project {j}", ProjectId = project.Id };
                        tasks.Add(task);
                        _context.Tasks.Add(task);
                        await _context.SaveChangesAsync();

                        // Create 3 months of timesheet records per Task
                        for (int m = 0; m < 90; m++)
                        {
                            var timesheet = new TimesheetEntry
                            {
                                TaskId = task.Id,
                                Date = DateTime.UtcNow.AddDays(-m),
                                HoursWorked = _random.Next(1, 9),
                                Description = $"Work done on {DateTime.UtcNow.AddDays(-m):yyyy-MM-dd}",
                                UserId = _random.Next(1, 101) // Assign random user
                            };
                            timesheetEntries.Add(timesheet);
                            _context.TimesheetEntries.Add(timesheet);
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();

            // Create 100 Users
            for (int i = 1; i <= 100; i++)
            {
                var user = new User { Name = $"User {i}", Email = $"user{i}@example.com" };
                users.Add(user);
                _context.Users.Add(user);
            }
            await _context.SaveChangesAsync();

            // Assign Users to Clients (5-15 users per Client)
            foreach (var client in clients)
            {
                int numUsers = _random.Next(5, 16);
                var assignedUsers = users.OrderBy(x => _random.Next()).Take(numUsers).ToList();

                foreach (var user in assignedUsers)
                {
                    var userClient = new UserClient { UserId = user.Id, ClientId = client.Id };
                    userClients.Add(userClient);
                    _context.UserClients.Add(userClient);
                }
            }

            await _context.SaveChangesAsync();

            return Ok("Database populated successfully!");
        }
    }
}
