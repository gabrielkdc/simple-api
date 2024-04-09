using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;
using UsersAPI.Models;
using System;
using UsersAPI.Services;
using UsersAPI.Services.Users;

namespace UsersAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private RegisterUserService registerUserService;
    private UpdateUserService updateUserService;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
        this.registerUserService = new RegisterUserService(context);
        this.updateUserService = new UpdateUserService(context);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser(User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Datos de usuario no v�lidos.");
        }

        var registerResult = await registerUserService.CreateNewUser(user);

        switch (registerResult)
        {
            case 0 :
                return BadRequest("La contraseña no puede estar vacía.");
            case 1:
                return Conflict("El nombre de usuario ya est� en uso.");
            case 2:
                return Ok(user);
            default:
                return StatusCode(500, "Error al registrar el usuario.");
        }
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
        var updateResult = await updateUserService.UpdateUser(id, user);

        switch (updateResult)
        {
            case 0:
                return BadRequest("ID del usuario no coincide con el ID proporcionado en la URL.");
            case 1:
                return NotFound("Usuario no encontrado.");
            case 2:
                return Ok(user);
            default:
                return BadRequest();
        }   
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
    

