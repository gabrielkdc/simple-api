using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;
using UsersAPI.Models;
using UsersAPI.RepositoryAbstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace UsersAPI.Repositorios;

public class UsersRepository : IUsersRepository
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

    public async Task<bool> UserExists(int id)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
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
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }


    public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }


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
        var query = GetOrderByQuery(orderBy);

        return await query.ToListAsync();
    }

    public IQueryable<User> GetOrderByQuery(string orderBy)
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

        return query;
    }

    public async Task<bool> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public string GenerateSQL()
    {
        return string.Empty;
    }
    
    

}