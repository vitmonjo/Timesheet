using System.Net.Http.Json;
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
}
