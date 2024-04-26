using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;
using UsersAPI.Models;
using UsersAPI.Repositorios;
using UsersAPI.RepositoryAbstractions;
using UsersAPI.ServiceAbstractions;

namespace UsersAPI.Services.Users
{
    public class GetUserByUsernameService : IGetUserByUsernameService
    {
        private readonly IUsersRepository usersRepository;

        public GetUserByUsernameService(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await usersRepository.GetUserByUsername(username);
        }
    }
}
