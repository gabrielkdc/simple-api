using UsersAPI.Models;

namespace UsersAPI.RepositoryAbstractions;

public interface IUsersRepository
{
    Task<bool> Create(User user);
    Task<bool> UserExists(string username);
    Task<bool> UserExists(int id);
    Task<bool> Update(User user);
    Task<bool> DeleteUser(int id);
}