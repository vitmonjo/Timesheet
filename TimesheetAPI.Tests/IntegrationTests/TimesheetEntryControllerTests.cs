using System.Net.Http.Json;
using TimesheetAPI.DTOs;
using Xunit;
using Xunit.Abstractions;

namespace TimesheetAPI.Tests.IntegrationTests;

public class TimesheetEntryTests : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly List<int> _createdTimesheetEntryIds = new();

    public TimesheetEntryTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    public Task InitializeAsync()
    {
        // Runs before the first test in this class (useful for setting up test data)
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        // Cleanup: Delete all test records after tests run
        foreach (var id in _createdTimesheetEntryIds)
        {
            await _client.DeleteAsync($"/api/TimesheetEntry/{id}");
        }
    }

    [Fact]
    public async Task CreateTimesheetEntry_ShouldReturnCreated()
    {
        // Arrange
        var TimesheetEntry = new
        {
            UserId = 1,
            TaskId = 2,
            Date = DateTime.Now,
            HoursWorked = 5,
            Description = "TimesheetEntry 1"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/TimesheetEntry", TimesheetEntry);
        var createdTimesheetEntry = await response.Content.ReadFromJsonAsync<TimesheetEntryDTO>();

        if (createdTimesheetEntry != null)
        {
            _createdTimesheetEntryIds.Add(createdTimesheetEntry.Id); // Track ID for cleanup
        }

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetTimesheetEntryById_ShouldReturnOk()
    {
        // Arrange
        var TimesheetEntry = new
        {
            UserId = 1,
            TaskId = 2,
            Date = DateTime.Now,
            HoursWorked = 5,
            Description = "TimesheetEntry 1"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/TimesheetEntry", TimesheetEntry);
        var responseContent = await createResponse.Content.ReadAsStringAsync();

        var createdTimesheetEntry = await createResponse.Content.ReadFromJsonAsync<TimesheetEntryDTO>();

        if (createdTimesheetEntry != null)
        {
            _createdTimesheetEntryIds.Add(createdTimesheetEntry.Id); // Track ID for cleanup
        }

        // Act
        var response = await _client.GetAsync($"/api/TimesheetEntry/{createdTimesheetEntry?.Id}");
        var getResponseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateTimesheetEntry_ShouldReturnOk()
    {
        // Arrange
        var TimesheetEntry = new
        {
            UserId = 1,
            TaskId = 2,
            Date = DateTime.Now,
            HoursWorked = 5,
            Description = "TimesheetEntry 1"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/TimesheetEntry", TimesheetEntry);
        var createdTimesheetEntry = await createResponse.Content.ReadFromJsonAsync<TimesheetEntryDTO>();

        if (createdTimesheetEntry != null)
        {
            _createdTimesheetEntryIds.Add(createdTimesheetEntry.Id); // Track ID for cleanup
        }

        // Arrange
        var updatedTimesheetEntry = new
        {
            Name = "TimesheetEntry 2",
            ClientId = 3
        };

        // Act
        var response = await _client.PatchAsJsonAsync($"/api/TimesheetEntry/{createdTimesheetEntry.Id}", updatedTimesheetEntry);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteTimesheetEntry_ShouldReturnOk()
    {
        // Arrange
        var TimesheetEntry = new
        {
            UserId = 1,
            TaskId = 2,
            Date = DateTime.Now,
            HoursWorked = 5,
            Description = "TimesheetEntry 1"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/TimesheetEntry", TimesheetEntry);
        var createdTimesheetEntry = await createResponse.Content.ReadFromJsonAsync<TimesheetEntryDTO>();

        // Act
        var response = await _client.DeleteAsync($"/api/TimesheetEntry/{createdTimesheetEntry.Id}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var getResponse = await _client.GetAsync($"/api/TimesheetEntry/{createdTimesheetEntry.Id}");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}

