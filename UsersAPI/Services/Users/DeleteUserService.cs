using UsersAPI.Data;
using UsersAPI.Models;
using UsersAPI.Repositorios;
using UsersAPI.RepositoryAbstractions;
using UsersAPI.ServiceAbstractions;

namespace UsersAPI.Services.Users
{
    public class DeleteUserService : IDeleteUserService
    {
        private IUsersRepository usersRepository;

        public DeleteUserService(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await usersRepository.DeleteUser(id);
        }
    }
}
