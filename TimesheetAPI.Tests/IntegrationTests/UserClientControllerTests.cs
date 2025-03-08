using System.Net.Http.Json;
using TimesheetAPI.DTOs;
using Xunit;

namespace TimesheetAPI.Tests.IntegrationTests;

public class UserClientControllerTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public UserClientControllerTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
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

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetUserClientById_ShouldReturnOk()
    {
        // Arrange
        var userClient = new
        {
            UserId = 1,
            ClientId = 2
        };

        var createResponse = await _client.PostAsJsonAsync("/api/UserClient", userClient);
        var createdUserClient = await createResponse.Content.ReadFromJsonAsync<UserClientDTO>();

        // Act
        var response = await _client.GetAsync($"/api/UserClient/{createdUserClient.Id}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        var fetchedUserClient = await response.Content.ReadFromJsonAsync<UserClientDTO>();
        Assert.Equal(userClient.UserId, fetchedUserClient.UserId);
        Assert.Equal(userClient.ClientId, fetchedUserClient.ClientId);
    }

    //[Fact]
    //public async Task UpdateUserClient_ShouldReturnOk()
    //{
    //    // Arrange
    //    var userClient = new
    //    {
    //        UserId = 1,
    //        ClientId = 2
    //    };

    //    var createResponse = await _client.PostAsJsonAsync("/api/UserClient", userClient);
    //    var createdUserClient = await createResponse.Content.ReadFromJsonAsync<UserClientDTO>();

    //    var updatedUserClient = new
    //    {
    //        UserId = 3,
    //        ClientId = 4
    //    };

    //    // Act
    //    var response = await _client.PutAsJsonAsync($"/api/UserClient/{createdUserClient.Id}", updatedUserClient);

    //    // Assert
    //    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    //}

    //[Fact]
    //public async Task DeleteUserClient_ShouldReturnOk()
    //{
    //    // Arrange
    //    var userClient = new
    //    {
    //        UserId = 1,
    //        ClientId = 2
    //    };

    //    var createResponse = await _client.PostAsJsonAsync("/api/UserClient", userClient);
    //    var createdUserClient = await createResponse.Content.ReadFromJsonAsync<UserClientDTO>();

    //    // Act
    //    var response = await _client.DeleteAsync($"/api/UserClient/{createdUserClient.Id}");

    //    // Assert
    //    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

    //    var getResponse = await _client.GetAsync($"/api/UserClient/{createdUserClient.Id}");
    //    Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
    //}
}
