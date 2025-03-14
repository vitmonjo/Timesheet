using System.Net.Http.Json;
using TimesheetAPI.DTOs;
using Xunit;
using Xunit.Abstractions;

namespace TimesheetAPI.Tests.IntegrationTests;

public class ClientTests : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly List<int> _createdClientIds = new();

    public ClientTests(TestWebApplicationFactory factory)
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
        foreach (var id in _createdClientIds)
        {
            await _client.DeleteAsync($"/api/Client/{id}");
        }
    }

    [Fact]
    public async Task CreateClient_ShouldReturnCreated()
    {
        // Arrange
        var Client = new
        {
            Name = "Client 1",
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Client", Client);
        var createdClient = await response.Content.ReadFromJsonAsync<ClientDTO>();

        if (createdClient != null)
        {
            _createdClientIds.Add(createdClient.Id); // Track ID for cleanup
        }

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetClientById_ShouldReturnOk()
    {
        // Arrange
        var Client = new
        {
            Name = "Client 1",
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Client", Client);
        var responseContent = await createResponse.Content.ReadAsStringAsync();

        var createdClient = await createResponse.Content.ReadFromJsonAsync<ClientDTO>();

        if (createdClient != null)
        {
            _createdClientIds.Add(createdClient.Id); // Track ID for cleanup
        }

        // Act
        var response = await _client.GetAsync($"/api/Client/{createdClient?.Id}");
        var getResponseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateClient_ShouldReturnOk()
    {
        // Arrange
        var Client = new
        {
            Name = "Client 1",
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Client", Client);
        var createdClient = await createResponse.Content.ReadFromJsonAsync<ClientDTO>();

        if (createdClient != null)
        {
            _createdClientIds.Add(createdClient.Id); // Track ID for cleanup
        }

        // Arrange
        var updatedClient = new
        {
            Name = "Client 2",
        };

        // Act
        var response = await _client.PatchAsJsonAsync($"/api/Client/{createdClient.Id}", updatedClient);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteClient_ShouldReturnOk()
    {
        // Arrange
        var Client = new
        {
            Name = "Client 1",
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Client", Client);
        var createdClient = await createResponse.Content.ReadFromJsonAsync<ClientDTO>();

        // Act
        var response = await _client.DeleteAsync($"/api/Client/{createdClient.Id}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var getResponse = await _client.GetAsync($"/api/Client/{createdClient.Id}");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}

