using UsersAPI.Models;

namespace UsersAPI.ServiceAbstractions
{
    public interface IGetUserByIdService
    {
        Task<User> GetUserById(int id);
    }
}
