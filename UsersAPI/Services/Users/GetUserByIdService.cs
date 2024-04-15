using UsersAPI.Data;
using UsersAPI.Models;
using UsersAPI.Repositorios;

namespace UsersAPI.Services.Users
{
    public class GetUserByIdService
    {
        private UsersRepository usersRepository;

        public GetUserByIdService(ApplicationDbContext context)
        {
            this.usersRepository = new UsersRepository(context);
        }

        public async Task<User> GetUserById(int id)
        {
            return await usersRepository.GetUserById(id);
        }
    }
}
