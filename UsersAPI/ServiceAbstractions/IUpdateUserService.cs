using UsersAPI.Enums;
using UsersAPI.Models;

namespace UsersAPI.ServiceAbstractions;

public interface IUpdateUserService
{
    Task<ResultCode> UpdateUser(int Id, User user);
}

