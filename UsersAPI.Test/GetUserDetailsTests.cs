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

public class GetUserDetailsTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    private readonly HttpClient httpClient;
    readonly List<int> createdUsersIds = new List<int>();
    public GetUserDetailsTests(WebApplicationFactory<IApiMarker> webApplicationFactory)
    {
        httpClient = webApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task GetUserDetail_ShouldReturnNotFound_WhenTheUserDoesNotExists()
    {
        // Arrange -- prepare the test data

        var newUser = new User
        {
            Name = "Nombre",
            Password = "12345678",
            Username = "testUser"
        };

        //Act -- Call the method to create the user

        var createResponse = await httpClient.PostAsJsonAsync("Users", newUser);
        createResponse.EnsureSuccessStatusCode();
        var createdUser = await createResponse.Content.ReadFromJsonAsync<User>();
        var userId = createdUser.Id;
        var deleteResponse = await httpClient.DeleteAsync($"Users/{userId}");

        //Act -- Call the method to be tested

        var result = await httpClient.GetAsync($"Users/{userId}");

        //Assert -- Validate the result

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task GetUserDetail_ShouldReturnOK_WhenTheUseExists()
    {
        // Arrange -- prepare the test data

        var newUser = new User
        {
            Name = "Nombre",
            Password = "12345678",
            Username = "testUser"
        };

        //Act -- Call the method to create the user

        var createResponse = await httpClient.PostAsJsonAsync("Users", newUser);
        createResponse.EnsureSuccessStatusCode();
        var user = await createResponse.Content.ReadFromJsonAsync<User>();
        var userId = user.Id;

        //Act -- Call the method to be tested

        var result = await httpClient.GetAsync($"Users/{userId}");

        //Assert -- Validate the result

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        var createdUser = await result.Content.ReadFromJsonAsync<User>();
        Assert.NotNull(createdUser);
        createdUsersIds.Add(createdUser.Id);
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

