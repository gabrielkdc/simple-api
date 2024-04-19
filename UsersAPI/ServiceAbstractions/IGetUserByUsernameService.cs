using UsersAPI.Models;

namespace UsersAPI.ServiceAbstractions
{
    public interface IGetUserByUsernameService
    {
        Task<User> GetUserByUsername(String username);
    }
}