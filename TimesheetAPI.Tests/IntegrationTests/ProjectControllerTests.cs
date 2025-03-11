using System.Net.Http.Json;
using TimesheetAPI.DTOs;
using Xunit;
using Xunit.Abstractions;

namespace TimesheetAPI.Tests.IntegrationTests;

public class ProjectTests : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly List<int> _createdProjectIds = new();

    public ProjectTests(TestWebApplicationFactory factory)
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
        foreach (var id in _createdProjectIds)
        {
            await _client.DeleteAsync($"/api/Project/{id}");
        }
    }

    [Fact]
    public async Task CreateProject_ShouldReturnCreated()
    {
        // Arrange
        var Project = new
        {
            Name = "Project 1",
            ClientId = 2
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Project", Project);
        var createdProject = await response.Content.ReadFromJsonAsync<ProjectDTO>();

        if (createdProject != null)
        {
            _createdProjectIds.Add(createdProject.Id); // Track ID for cleanup
        }

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetProjectById_ShouldReturnOk()
    {
        // Arrange
        var Project = new
        {
            Name = "Project 1",
            ClientId = 2
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Project", Project);
        var responseContent = await createResponse.Content.ReadAsStringAsync();

        var createdProject = await createResponse.Content.ReadFromJsonAsync<ProjectDTO>();

        if (createdProject != null)
        {
            _createdProjectIds.Add(createdProject.Id); // Track ID for cleanup
        }

        // Act
        var response = await _client.GetAsync($"/api/Project/{createdProject?.Id}");
        var getResponseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProject_ShouldReturnOk()
    {
        // Arrange
        var Project = new
        {
            Name = "Project 1",
            ClientId = 2
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Project", Project);
        var createdProject = await createResponse.Content.ReadFromJsonAsync<ProjectDTO>();

        if (createdProject != null)
        {
            _createdProjectIds.Add(createdProject.Id); // Track ID for cleanup
        }

        // Arrange
        var updatedProject = new
        {
            Name = "Project 2",
            ClientId = 3
        };

        // Act
        var response = await _client.PatchAsJsonAsync($"/api/Project/{createdProject.Id}", updatedProject);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteProject_ShouldReturnOk()
    {
        // Arrange
        var Project = new
        {
            Name = "Project 1",
            ClientId = 2
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Project", Project);
        var createdProject = await createResponse.Content.ReadFromJsonAsync<ProjectDTO>();

        // Act
        var response = await _client.DeleteAsync($"/api/Project/{createdProject.Id}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var getResponse = await _client.GetAsync($"/api/Project/{createdProject.Id}");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}

