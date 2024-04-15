using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Models;

namespace UsersAPI.Test;

public class DeleteUserTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    private readonly HttpClient httpClient;
    readonly List<int> createdUsersIds = new List<int>();
    public DeleteUserTests(WebApplicationFactory<IApiMarker>webApplicationFactory)
    {
        httpClient = webApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnNotFound_WhenTheUserDoNotExists()
    {
        // Arrange -- prepare the test data
        var newUser = new User
        {
            Name = "Nombre",
            Password = "12345678",
            Username = "TestUser1"   
        };
        // Act -- call the method to create the user

        var createResponse = await httpClient.PostAsJsonAsync("Users", newUser);
        createResponse.EnsureSuccessStatusCode();
        var createdUser = await createResponse.Content.ReadFromJsonAsync<User>();
        var userId = createdUser.Id;
        var deleteResponse = await httpClient.DeleteAsync($"Users/{userId}");

        // Act -- call the method to be tested

        var result = await httpClient.GetAsync($"Users/{userId}");

        // Assert -- Validate the result

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
     public async Task DeleteUser_ShouldReturnOK_WhenAnUserIsDeleted()
    {
        // Arrange -- prepare the test data
        var newuser = new User
        {
            Name = "NOMBRE",
            Username = "testuser2",
            Password = "12345678",
        };

        // Act -- call the method to create the user

        var createdRespose = await httpClient.PostAsJsonAsync("Users", newuser);
        createdRespose.EnsureSuccessStatusCode();
        var createdUser = await createdRespose.Content.ReadFromJsonAsync<User>();
        var userId = createdUser.Id;

        // Act -- call the method to be tested

        var result = await httpClient.DeleteAsync($"Users/{userId}");

        //Assert -- Validate the result

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
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
