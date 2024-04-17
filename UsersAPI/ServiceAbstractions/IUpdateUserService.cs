using UsersAPI.Models;

namespace UsersAPI.ServiceAbstractions;

public interface IUpdateUserService
{
    Task<int> UpdateUser(int Id, User user);
}

