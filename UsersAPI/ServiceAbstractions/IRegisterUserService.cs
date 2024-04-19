using UsersAPI.Enums;
using UsersAPI.Models;

namespace UsersAPI.ServiceAbstractions;

public interface IRegisterUserService
{
    Task<ResultCode> CreateNewUser(User user);
}