using UsersAPI.Enums;
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

    public async Task<ResultCode> CreateNewUser(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Password))
        {
            return ResultCode.INVALID_INPUT;
        }

        user.Name = user.Name?.Trim();
        user.Username = user.Username?.Trim();

        var existingUser = await usersRepository.UserExists(user.Username);
        if (existingUser)
        {
            return ResultCode.RECORDS_CONFLICT;
        }

        await usersRepository.Create(user);
        return ResultCode.SUCCESS;
    }


}


