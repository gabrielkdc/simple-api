using UsersAPI.Data;
using UsersAPI.Models;
using UsersAPI.Repositorios;

namespace UsersAPI.Services.Users
{
    public class DeleteUserService
    {
        private UsersRepository usersRepository;

        public DeleteUserService(ApplicationDbContext context)
        {
            this.usersRepository = new UsersRepository(context);
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await usersRepository.DeleteUser(id);
        }
    }
}
