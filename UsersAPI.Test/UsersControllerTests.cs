using System.Net;
using System.Net.Http.Json;
using UsersAPI.Models;

namespace UsersAPI.Test;

public class UsersControllerTests
{
    [Fact]
    public async Task RegisterUser_ShouldReturnOk_WhenAllFieldsAreFilled()
    {
        // Arrange  -- Prepare the test data

        var r = new Random(DateTime.Now.Millisecond);

        var newUser = new User
        {
            Name = "TestUser",
            Username = $"testuser{r.NextDouble()}",
            Password = "123456"
        };

        var httpClient = new HttpClient();

        // Act -- Call the method to be tested
        var result = await httpClient.PostAsJsonAsync("https://localhost:7266/Users", newUser);
        
        // Assert -- Validate the result
        
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

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

        var httpClient = new HttpClient();

        // Act -- Call the method to be tested
        var result = await httpClient.PostAsJsonAsync("https://localhost:7266/Users", newUser);
        
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

        var httpClient = new HttpClient();

        // Act -- Call the method to be tested
        var result = await httpClient.PostAsJsonAsync("https://localhost:7266/Users", newUser);
        
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

        var httpClient = new HttpClient();

        // Act -- Call the method to be tested
        var result = await httpClient.PostAsJsonAsync("https://localhost:7266/Users", newUser);

        // Assert -- Validate the result

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }
}