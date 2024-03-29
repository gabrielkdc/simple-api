using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;
using UsersAPI.Models;
using System;

namespace UsersAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser(User user)
    {
        // Verificar si el modelo es válido
        if (!ModelState.IsValid)
        {
            return BadRequest("Datos de usuario no v�lidos.");
        }

        // Validar que el campo Password no esté compuesto únicamente por espacios vacíos
        if (string.IsNullOrWhiteSpace(user.Password))
        {
            return BadRequest("La contraseña no puede estar vacía.");
        }

        // Eliminar espacios vacíos al principio y al final de los campos Name y Username
        user.Name = user.Name?.Trim();
        user.Username = user.Username?.Trim();

        // Verificar si ya existe un usuario con el mismo nombre de usuario
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
        if (existingUser != null)
        {
            return Conflict("El nombre de usuario ya est� en uso.");
        }

        // Agregar el usuario a la base de datos
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(user);
    }
    
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserDetails(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound("Usuario no encontrado.");
        }

        return Ok(user);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {

        if (!UserExists(id))
        {
            return NotFound("Usuario no encontrado.");
        }

        var user = await _context.Users.FindAsync(id);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok("Usuario eliminado exitosamente.");
    }
    
    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest("ID del usuario no coincide con el ID proporcionado en la URL.");
        }

        _context.Entry(user).State = EntityState.Modified;


        if (!UserExists(id))
        {
            return NotFound("Usuario no encontrado.");
        }
        else
        {
            await _context.SaveChangesAsync();
        }


        return Ok("Usuario actualizado exitosamente.");
    }


    [HttpGet]
    public async Task<IActionResult> GetUsers(string orderBy = "username")
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
            default:
                return BadRequest("El par�metro 'orderBy' solo puede ser 'username' o 'name'.");
        }

        var users = await query.ToListAsync();
        return Ok(users);
    }

    [HttpGet("username/{username}")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            return NotFound("Usuario no encontrado.");
        }

        return Ok(user);
    }
}
    

