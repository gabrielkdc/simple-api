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
    public async Task ShouldReturnNotfoun_WhenTheUserDoesNotExists()
    {
        // Arrange -- prepare the test data

        var newUser = new User
        {
            Name="Nombre",
            Password="1234567",
            Username="Username2"
        };
        await httpClient.PostAsJsonAsync("Users", newUser);
        var UserId = newUser.Id;
        httpClient.DeleteAsync($"Users/{UserId}");

        // Act call the method to be tested

        var result = await httpClient.GetAsync($"/Users/{UserId}");

        // Asert -- Validate the Result

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

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

