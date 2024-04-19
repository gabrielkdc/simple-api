namespace UsersAPI.ServiceAbstractions
{
    public interface IDeleteUserService
    {
        Task<bool> DeleteUser(int id);
    }
}
