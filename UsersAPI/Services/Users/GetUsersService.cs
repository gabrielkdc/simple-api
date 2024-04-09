using UsersAPI.Data;
using UsersAPI.Repositorios;

namespace UsersAPI.Services.Users
{
    public class GetUsersService
    {
        private UsersRepository _usersRepository;

        public  GetUsersService(ApplicationDbContext context)
        {
            this._usersRepository = new UsersRepository(context);
        }

        public async Task<int> GetUsers(string orderBy)
        {

            orderBy = orderBy.ToLower();

            if (orderBy != "username" && orderBy != "name")
            {
                return 0;
            }

            await _usersRepository.GetUsers(orderBy);
            return 1;
        }
    }
}
