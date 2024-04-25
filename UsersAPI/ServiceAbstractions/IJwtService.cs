using UsersAPI.Models;

namespace UsersAPI.ServiceAbstractions
{
    public interface IJwtService
    {
        Task<string> GenerateJwtToken(User user); 
    }
}
