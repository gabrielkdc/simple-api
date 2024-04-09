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

    public async Task<bool> Update(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<User>> GetUsers(string orderBy)
    {
        List<User> userList;
        switch (orderBy.ToLower())
        {
            case "username":
                userList = await _context.Users.OrderBy(u => u.Username).ToListAsync();
                break;
            case "name":
                userList = await _context.Users.OrderBy(u => u.Name).ToListAsync();
                break;
            default:
                userList = await _context.Users.ToListAsync();
                break;
        }
        return userList;
    }
}