using UsersAPI.Models;

namespace UsersAPI.ServiceAbstractions;

public interface IRegisterUserService
{
    Task<int> CreateNewUser(User user);
}