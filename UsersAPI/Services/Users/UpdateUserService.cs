using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;
using UsersAPI.Models;
using UsersAPI.Repositorios;
using UsersAPI.RepositoryAbstractions;
using UsersAPI.ServiceAbstractions;

namespace UsersAPI.Services.Users
{
    public class UpdateUserService : IUpdateUserService
    {
        private IUsersRepository usersRepository;

        public UpdateUserService(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task<int> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return 0;
            }

            if (! await usersRepository.UserExists(id))
            {
                return 1;
            }
            else
            {
                await usersRepository.Update(user);
                return 2;
            }
        }
    }
}
