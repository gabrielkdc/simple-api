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
<<<<<<< Updated upstream
=======

    public async Task<bool> Update(User user)
    {
        try{
            var existingEntity = _context.ChangeTracker.Entries<User>().FirstOrDefault(e => e.Entity.Id == user.Id);
            if (existingEntity != null) 
            { 
                _context.Entry(existingEntity.Entity).State = EntityState.Detached; 
            }

            _context.Entry(user).State = EntityState.Modified;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<List<User>> GetUsers(string orderBy)
    {

        IQueryable<User> query = _context.Users;

        switch (orderBy.ToLower())
        {
            case "username":
                query = query.OrderBy(u => u.Username);
                break;
            case "name":
                query = query.OrderBy(u => u.Name);
                break;
        }

        return await query.ToListAsync();
    }
>>>>>>> Stashed changes
}