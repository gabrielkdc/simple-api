using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;
using UsersAPI.Models;
using UsersAPI.Repositorios;

namespace UsersAPI.Services.Users
{
    public class GetUserByUsernameService
    {
        private UsersRepository usersRepository;

        public GetUserByUsernameService(ApplicationDbContext context)
        {
            usersRepository = new UsersRepository(context);
        }

        public async Task<User> GetUserByUsername(string username)
        {

            var existingUser = await usersRepository.UserExists(username);

            var user = await usersRepository.GetUserByUsername(username);
            return user;
        }
    }
}
