using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;
using UsersAPI.Models;

namespace UsersAPI.Repositorios;

public class UsersRepository
{
    private readonly ApplicationDbContext _context;

    public UsersRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UserExists(string username)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        return existingUser != null;
    }

    public async Task<bool> Create(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<User> GetUserByUsername(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        return user;
    }
}