using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;
using UsersAPI.Models;
using UsersAPI.Repositorios;

namespace UsersAPI.Services;

public class RegisterUserService
{

    private UsersRepository usersRepository;


    public RegisterUserService(ApplicationDbContext context)
    {
        this.usersRepository = new UsersRepository(context);
    }

    public async Task<int> CreateNewUser(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Password))
        {
            return 0;
        }

        user.Name = user.Name?.Trim();
        user.Username = user.Username?.Trim();

        var existingUser = await usersRepository.UserExists( user.Username);
        if (existingUser)
        {
            return 1;
        }

        await usersRepository.Create(user);
        return 2;
    }
}