using Microsoft.EntityFrameworkCore;
using UsersAPI.Models;
using UsersAPI.Data;
using UsersAPI.Repositorios;

namespace UsersAPI.Services.Users;

public class UpdateUserService
{
    private UsersRepository usersRepository;


    public UpdateUserService(ApplicationDbContext context)
    {
        this.usersRepository = new UsersRepository(context);
    }

    public async Task<int> UpdateUser(User user)
    { 
        var existingUser = await usersRepository.UserExists(user.Username);
        if (!existingUser)
        {
            return 0;
        }

        if (string.IsNullOrWhiteSpace(user.Password))
        {
            return 1;
        }

        user.Name = user.Name?.Trim();
        user.Username = user.Username?.Trim();


        await usersRepository.Update(user);
        return 2;
    }
}

