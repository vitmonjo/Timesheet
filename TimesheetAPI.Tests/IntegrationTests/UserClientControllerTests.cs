using System.Net.Http.Json;
using TimesheetAPI.DTOs;
using Xunit;
using Xunit.Abstractions;

namespace TimesheetAPI.Tests.IntegrationTests;

public class UserClientTests : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly List<int> _createdUserClientIds = new();

    public UserClientTests(TestWebApplicationFactory factory)
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
        foreach (var id in _createdUserClientIds)
        {
            await _client.DeleteAsync($"/api/UserClient/{id}");
        }
    }

    [Fact]
    public async Task CreateUserClient_ShouldReturnCreated()
    {
        // Arrange
        var userClient = new
        {
            UserId = 1,
            ClientId = 2
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/UserClient", userClient);
        var createdUserClient = await response.Content.ReadFromJsonAsync<UserClientDTO>();

        if (createdUserClient != null)
        {
            _createdUserClientIds.Add(createdUserClient.Id); // Track ID for cleanup
        }

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetUserClientById_ShouldReturnOk()
    {
        // Arrange
        var userClient = new { UserId = 1, ClientId = 2 };

        var createResponse = await _client.PostAsJsonAsync("/api/UserClient", userClient);
        var responseContent = await createResponse.Content.ReadAsStringAsync();

        var createdUserClient = await createResponse.Content.ReadFromJsonAsync<UserClientDTO>();

        if (createdUserClient != null)
        {
            _createdUserClientIds.Add(createdUserClient.Id); // Track ID for cleanup
        }

        // Act
        var response = await _client.GetAsync($"/api/UserClient/{createdUserClient?.Id}");
        var getResponseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateUserClient_ShouldReturnOk()
    {
        // Arrange
        var userClient = new
        {
            UserId = 3,
            ClientId = 4
        };

        var createResponse = await _client.PostAsJsonAsync("/api/UserClient", userClient);
        var createdUserClient = await createResponse.Content.ReadFromJsonAsync<UserClientDTO>();

        if (createdUserClient != null)
        {
            _createdUserClientIds.Add(createdUserClient.Id); // Track ID for cleanup
        }

        var updatedUserClient = new
        {
            UserId = 5,
            ClientId = 6
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/UserClient/{createdUserClient.Id}", updatedUserClient);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteUserClient_ShouldReturnOk()
    {
        // Arrange
        var userClient = new
        {
            UserId = 7,
            ClientId = 8
        };

        var createResponse = await _client.PostAsJsonAsync("/api/UserClient", userClient);
        var createdUserClient = await createResponse.Content.ReadFromJsonAsync<UserClientDTO>();

        // Act
        var response = await _client.DeleteAsync($"/api/UserClient/{createdUserClient.Id}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var getResponse = await _client.GetAsync($"/api/UserClient/{createdUserClient.Id}");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}

