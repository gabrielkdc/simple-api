using System.Net;
using System.Net.Http.Json;
using UsersAPI.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace UsersAPI.Test;

public class UsersControllerTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    private readonly HttpClient httpClient;
    readonly List<int> createdUsersIds = new List<int>();
    public UsersControllerTests(WebApplicationFactory<IApiMarker> webApplicationFactory)
    {
        httpClient = webApplicationFactory.CreateClient();
    }
    
    [Theory]
    [InlineData("TestUser", "testuser", "123456")]
    [InlineData("TestUser1", "testuser1", "123456ased")]
    [InlineData("TestUser2", "testuser2", "123456dds")]
    [InlineData("TestUser3", "testuser3", "123456de")]
    public async Task RegisterUser_ShouldReturnOk_WhenAllFieldsAreFilledWithDifferentScenarios(string name, string username, string password )
    {
        // Arrange  -- Prepare the test data
        var newUser = new User
        {
            Name = name,
            Username = username,
            Password = password
        };

        // Act -- Call the method to be tested
        var result = await httpClient.PostAsJsonAsync("Users", newUser);
        
        // Assert -- Validate the result
        
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        var createdUser = await result.Content.ReadFromJsonAsync<User>();
        Assert.NotNull(createdUser);
        Assert.Equal(newUser.Name,createdUser.Name);
        Assert.Equal(newUser.Username,createdUser.Username);
        createdUsersIds.Add(createdUser.Id);
    }
    
    [Theory]
    [InlineData("TestUser", "testuser", "123")]
    [InlineData("TestUser1", "testuser1", " ")]
    [InlineData("TestUser2", "testuser2", "1204")]
    [InlineData("TestUser3", "testuser3", "12345")]
    [InlineData("TestUser3", "testuser3", "12345123456")]
    public async Task RegisterUser_ShouldFail_WhenAllPasswordLengthIsNotWithinTheValidRange(string name, string username, string password )
    {
        // Arrange  -- Prepare the test data
        var newUser = new User
        {
            Name = name,
            Username = username,
            Password = password
        };

        // Act -- Call the method to be tested
        var result = await httpClient.PostAsJsonAsync("Users", newUser);
        
        // Assert -- Validate the result
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }
    
    [Fact]
    public async Task RegisterUser_ShouldReturnOk_WhenAllFieldsAreFilled()
    {
        // Arrange  -- Prepare the test data
        var newUser = new User
        {
            Name = "TestUser",
            Username = "testuser",
            Password = "123456"
        };

        // Act -- Call the method to be tested
        var result = await httpClient.PostAsJsonAsync("Users", newUser);
        
        
        // Assert -- Validate the result
        
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        var createdUser = await result.Content.ReadFromJsonAsync<User>();
        Assert.NotNull(createdUser);
        createdUsersIds.Add(createdUser.Id);
    }
    
    [Fact]
    public async Task RegisterUser_ShouldFailWithPasswordEmtpy_WhenPasswordIsNull()
    {
        // Arrange  -- Prepare the test data

        var newUser = new User
        {
            Name = "TestUser",
            Username = "testuser"
        };
        
        // Act -- Call the method to be tested
        var result = await httpClient.PostAsJsonAsync("Users", newUser);
        
        // Assert -- Validate the result
        
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

    }
    
    [Fact]
    public async Task RegisterUser_ShouldFail_WhenPasswordIsLessThan6Characters()
    {
        // Arrange  -- Prepare the test data

        var newUser = new User
        {
            Name = "TestUser",
            Username = "testuser",
            Password = "12345"
        };
        
        // Act -- Call the method to be tested
        var result = await httpClient.PostAsJsonAsync("Users", newUser);
        
        // Assert -- Validate the result
        
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

    }

    [Fact]
    public async Task RegisterUser_ShouldFail_WhenPassordIsMoreThan10Characters()
    {

        // Arrange  -- Prepare the test data

        var newUser = new User
        {
            Name = "TestUser",
            Username = "testuser",
            Password = "1234567890a"
        };

        // Act -- Call the method to be tested
        var result = await httpClient.PostAsJsonAsync("Users", newUser);

        // Assert -- Validate the result

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task GetUserByUsername_ShouldReturnOk_WhenUserExists()
    {
        // Arrange
        var newUser = new User
        {
            Name = "Tester",
            Username = "tester",
            Password = "1234567"
        };
        var registrationResult = await httpClient.PostAsJsonAsync("Users", newUser);
        registrationResult.EnsureSuccessStatusCode(); // Ensure user registration was successful

        // Act
        var result = await httpClient.GetAsync($"Users/username/{newUser.Username}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        var user = await result.Content.ReadFromJsonAsync<User>();
        Assert.NotNull(user);
        Assert.Equal(newUser.Username, user.Username);
    }

    [Fact]
    public async Task GetUserByUsername_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        string nonExistentUsername = "nonexistentuser";

        // Act
        var response = await httpClient.GetAsync($"Users/username/{nonExistentUsername}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }


    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        var deleteTasks = new List<Task>();
        foreach (var userId in createdUsersIds)
        {
            deleteTasks.Add(httpClient.DeleteAsync($"Users/{userId}"));
        }
        await Task.WhenAll(deleteTasks);
        httpClient.Dispose();
    }
}