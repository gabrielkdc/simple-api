using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;
using UsersAPI.Enums;
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

        public async Task<ResultCode> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return ResultCode.INVALID_INPUT;
            }

            if (! await usersRepository.UserExists(id))
            {
                return ResultCode.RECORD_NOT_FOUND;
            }
            else
            {
                await usersRepository.Update(user);
                return ResultCode.SUCCESS;
            }
        }
    }
}
