using System.Net.Http.Json;
using TimesheetAPI.DTOs;
using Xunit;
using Xunit.Abstractions;

namespace TimesheetAPI.Tests.IntegrationTests;

public class TaskTests : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly List<int> _createdTaskIds = new();

    public TaskTests(TestWebApplicationFactory factory)
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
        foreach (var id in _createdTaskIds)
        {
            await _client.DeleteAsync($"/api/Task/{id}");
        }
    }

    [Fact]
    public async Task CreateTask_ShouldReturnCreated()
    {
        // Arrange
        var Task = new
        {
            Name = "Task 1",
            ProjectId = 2
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Task", Task);
        var createdTask = await response.Content.ReadFromJsonAsync<TaskDTO>();

        if (createdTask != null)
        {
            _createdTaskIds.Add(createdTask.Id); // Track ID for cleanup
        }

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetTaskById_ShouldReturnOk()
    {
        // Arrange
        var Task = new
        {
            Name = "Task 1",
            ProjectId = 2
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Task", Task);
        var responseContent = await createResponse.Content.ReadAsStringAsync();

        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskDTO>();

        if (createdTask != null)
        {
            _createdTaskIds.Add(createdTask.Id); // Track ID for cleanup
        }

        // Act
        var response = await _client.GetAsync($"/api/Task/{createdTask?.Id}");
        var getResponseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateTask_ShouldReturnOk()
    {
        // Arrange
        var Task = new
        {
            Name = "Task 1",
            ProjectId = 2
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Task", Task);
        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskDTO>();

        if (createdTask != null)
        {
            _createdTaskIds.Add(createdTask.Id); // Track ID for cleanup
        }

        // Arrange
        var updatedTask = new
        {
            Name = "Task 2",
            ProjectId = 3
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/Task/{createdTask.Id}", updatedTask);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteTask_ShouldReturnOk()
    {
        // Arrange
        var Task = new
        {
            Name = "Task 1",
            ProjectId = 2
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Task", Task);
        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskDTO>();

        // Act
        var response = await _client.DeleteAsync($"/api/Task/{createdTask.Id}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var getResponse = await _client.GetAsync($"/api/Task/{createdTask.Id}");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}

