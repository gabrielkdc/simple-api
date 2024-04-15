using UsersAPI.Models;

namespace UsersAPI.RepositoryAbstractions;

public interface IUsersRepository
{
    Task<bool> Create(User user);
    Task<bool> UserExists(string username);
}