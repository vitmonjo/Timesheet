using System.Net.Http.Json;
using TimesheetAPI.DTOs;
using Xunit;
using Xunit.Abstractions;

namespace TimesheetAPI.Tests.IntegrationTests;

public class UserTests : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly List<int> _createdUserIds = new();

    public UserTests(TestWebApplicationFactory factory)
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
        foreach (var id in _createdUserIds)
        {
            await _client.DeleteAsync($"/api/User/{id}");
        }
    }

    [Fact]
    public async Task CreateUser_ShouldReturnCreated()
    {
        // Arrange
        var User = new
        {
            Email = "user2@email.net",
            Name = "User 2"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/User", User);
        var createdUser = await response.Content.ReadFromJsonAsync<UserDTO>();

        if (createdUser != null)
        {
            _createdUserIds.Add(createdUser.Id); // Track ID for cleanup
        }

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetUserById_ShouldReturnOk()
    {
        // Arrange
        var User = new
        {
            Email = "user2@email.net",
            Name = "User 2"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/User", User);
        var responseContent = await createResponse.Content.ReadAsStringAsync();

        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDTO>();

        if (createdUser != null)
        {
            _createdUserIds.Add(createdUser.Id); // Track ID for cleanup
        }

        // Act
        var response = await _client.GetAsync($"/api/User/{createdUser?.Id}");
        var getResponseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnOk()
    {
        // Arrange
        var User = new
        {
            Email = "user1@email.net",
            Name = "User 1"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/User", User);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDTO>();

        if (createdUser != null)
        {
            _createdUserIds.Add(createdUser.Id); // Track ID for cleanup
        }

        // Arrange
        var updatedUser = new
        {
            Email = "user2@email.net",
            Name = "User 2"
        };

        // Act
        var response = await _client.PatchAsJsonAsync($"/api/User/{createdUser.Id}", updatedUser);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnOk()
    {
        // Arrange
        var User = new
        {
            Email = "user2@email.net",
            Name = "User 2"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/User", User);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserDTO>();

        // Act
        var response = await _client.DeleteAsync($"/api/User/{createdUser.Id}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var getResponse = await _client.GetAsync($"/api/User/{createdUser.Id}");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}

