using System.Net;
using System.Net.Http.Json;
using UsersAPI.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Channels;

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
            Username = "tester22",
            Password = "1234567"
        };
        var registrationResult = await httpClient.PostAsJsonAsync("Users", newUser);

        // Act
        var result = await httpClient.GetAsync($"Users/username/{newUser.Username}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        var user = await result.Content.ReadFromJsonAsync<User>();
        createdUsersIds.Add(user.Id);
        Assert.NotNull(user);
        createdUsersIds.Add(user.Id);
        Assert.Equal(newUser.Username, user.Username);
    }

    [Fact]
    public async Task GetUserByUsername_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange -- prepare the test data
        var newUser = new User
        {
            Name = "Nombre",
            Password = "12345678",
            Username = "TestUser5"
        };
        // Act -- call the method to create the user

        var createResponse = await httpClient.PostAsJsonAsync("Users", newUser);
        createResponse.EnsureSuccessStatusCode();
        var createdUser = await createResponse.Content.ReadFromJsonAsync<User>();
        var userId = createdUser.Id;
        var deleteResponse = await httpClient.DeleteAsync($"Users/{userId}");

        // Act
        var response = await httpClient.GetAsync($"Users/username/{newUser.Username}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnOk_WhenUserIsUpdated()

    {
        // Arrange
        var newUser = new User
        {
            Name = "TestUser",
            Username = "testuser4",
            Password = "123456"
        };

        // Agregar un nuevo usuario
        var result = await httpClient.PostAsJsonAsync("Users", newUser);
        var createdUser = await result.Content.ReadFromJsonAsync<User>();
        createdUsersIds.Add(createdUser.Id);

        // Datos del usuario actualizado
        var updatedUser = new User
        {
            Id = createdUser.Id,
            Name = "Tester",
            Username = "tester",
            Password = "newpasswo"
        };

        // Act - Llamar al m�todo para actualizar el usuario
        var updateResult = await httpClient.PutAsJsonAsync($"Users/{createdUser.Id}", updatedUser);

        // Assert - Validar el resultado
        updateResult.EnsureSuccessStatusCode(); // Verificar que la solicitud de actualizaci�n sea exitosa
        var updatedUserResult = await updateResult.Content.ReadFromJsonAsync<User>();
        Assert.NotNull(updatedUserResult);
        Assert.Equal(updatedUser.Name, updatedUserResult.Name);
        Assert.Equal(updatedUser.Username, updatedUserResult.Username);
    }

    [Fact]
    public async Task GetUsers_ShouldReturnListOfUsers()
    {
        // Arrange: prepara la solicitud GET con el par�metro orderBy
        var request = new HttpRequestMessage(HttpMethod.Get, "Users?orderBy=username");

        var newUser = new User
        {
            Name = "TestUser",
            Username = "testuser",
            Password = "123456"
        };
        var result = await httpClient.PostAsJsonAsync("Users", newUser);
        
        var createdUser = await result.Content.ReadFromJsonAsync<User>();
        createdUsersIds.Add(createdUser.Id);
        
        // Act: realiza la solicitud al servidor
        var response = await httpClient.SendAsync(request);

        // Assert: verifica que la solicitud sea exitosa y que devuelva una lista de usuarios
        response.EnsureSuccessStatusCode();
        var users = await response.Content.ReadFromJsonAsync<List<User>>();
        Assert.NotNull(users);
        Assert.NotEmpty(users);
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