using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;
using UsersAPI.Models;
using UsersAPI.Repositorios;
using UsersAPI.RepositoryAbstractions;
using UsersAPI.ServiceAbstractions;

namespace UsersAPI.Services.Users;
    public class GetUsersService : IGetUsersService
    {
        private readonly IUsersRepository usersRepository;

        public GetUsersService(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        }

        public async Task<List<User>> GetUsers(string orderBy)
        {

            if (orderBy.ToLower() != "username" && orderBy.ToLower() != "name")
            {
                throw new ArgumentException("El orderBy solo acepta 'name' o 'username'");
            }

            return await usersRepository.GetUsers(orderBy); 
        }
    }
