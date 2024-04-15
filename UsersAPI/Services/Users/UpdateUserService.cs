using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;
using UsersAPI.Models;
using UsersAPI.Repositorios;

namespace UsersAPI.Services.Users
{
    public class UpdateUserService
    {
        private UsersRepository _usersRepository;
        
        public UpdateUserService(ApplicationDbContext context)
        {
            _usersRepository = new UsersRepository(context);
        }

        public async Task<int> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return 0;
            }

            if (! await _usersRepository.UserExists(id))
            {
                return 1;
            }
            else
            {
                await _usersRepository.Update(user);
                return 2;
            }
        }
    }
}
