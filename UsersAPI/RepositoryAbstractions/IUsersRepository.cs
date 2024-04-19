using UsersAPI.Models;

namespace UsersAPI.RepositoryAbstractions;

public interface IUsersRepository
{
    Task<bool> UserExists(string username);
    Task<bool> UserExists(int id);
    Task <bool> Create(User user);
    Task<bool> Update(User user);
    Task<User> GetUserById(int id);


}