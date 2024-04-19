using UsersAPI.Data;
using UsersAPI.Models;
using UsersAPI.Repositorios;
using UsersAPI.RepositoryAbstractions;
using UsersAPI.ServiceAbstractions;

namespace UsersAPI.Services.Users
{
    public class GetUserByIdService : IGetUserByIdService
    {
        private IUsersRepository usersRepository;

        public GetUserByIdService(IUsersRepository userRepository)
        {
            this.usersRepository = userRepository;
        }

        public async Task<User> GetUserById(int id)
        {
            return await usersRepository.GetUserById(id);
        }
    }
}
