using UsersAPI.Models;

namespace UsersAPI.ServiceAbstractions
{
    public interface IGetUsersService
    {
        Task<List<User>> GetUsers(string orderBy);
    }
}
