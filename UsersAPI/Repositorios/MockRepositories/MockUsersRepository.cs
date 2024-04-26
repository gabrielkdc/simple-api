using Microsoft.EntityFrameworkCore;
using UsersAPI.Models;
using UsersAPI.RepositoryAbstractions;

namespace UsersAPI.Repositorios.MockRepositories;

public class MockUsersRepository : IUsersRepository
{
    public Task<bool> Create(User user)
    {
        return Task.FromResult(true);
    }
    public Task<bool> UserExists(int id)
    {
        return Task.FromResult(false);
    }
    public Task<bool> UserExists(string username)
    {
        return Task.FromResult(false);
    }
    public Task<bool> Update(User user)
    {
        return Task.FromResult(true);
    }
    public Task<User> GetUserById(int id)
    {
        var user = new User();
        return Task.FromResult(user);
    }

    public Task<User> GetUserByUsername(string username)
    {
        throw new NotImplementedException();
    }


    public Task<List<User>> GetUsers(string orderBy)
    {
        throw new NotImplementedException();
    }
    public Task<bool> DeleteUser(int id)
    {
        throw new NotImplementedException();
    }
}