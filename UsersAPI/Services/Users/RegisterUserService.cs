using UsersAPI.Models;
using UsersAPI.RepositoryAbstractions;
using UsersAPI.ServiceAbstractions;

namespace UsersAPI.Services;

public class RegisterUserService : IRegisterUserService
{

    private IUsersRepository usersRepository;


    public RegisterUserService(IUsersRepository usersRepository)
    {
        this.usersRepository = usersRepository;
    }

    public async Task<int> CreateNewUser(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Password))
        {
            return 0;
        }

        user.Name = user.Name?.Trim();
        user.Username = user.Username?.Trim();

        var existingUser = await usersRepository.UserExists(user.Username);
        if (existingUser)
        {
            return 1;
        }

        await usersRepository.Create(user);
        return 2;
    }


}


