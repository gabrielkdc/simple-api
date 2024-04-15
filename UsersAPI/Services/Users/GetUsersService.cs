using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;
using UsersAPI.Models;
using UsersAPI.Repositorios;

namespace UsersAPI.Services.Users;
    public class GetUsersService
    {
        private UsersRepository usersRepository;

        public GetUsersService(ApplicationDbContext context)
        {
            this.usersRepository = new UsersRepository(context);
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
