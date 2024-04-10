using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;
using UsersAPI.Models;
using System;
using UsersAPI.Services;

namespace UsersAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private RegisterUserService registerUserService;
    private UpdateUserService updateUserService;
    private GetUsersService getUserService;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
        this.registerUserService = new RegisterUserService(context);
        this.updateUserService = new UpdateUserService(context);
        this.getUserService = new GetUsersService(context);
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


        return Ok(user);
    }


    [HttpGet]
    public async Task<IActionResult> GetUsers(string orderBy = "username")
    {
        try
        {
            var getUsersResult = await getUserService.GetUsers(orderBy);
            return Ok(getUsersResult);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al obtener la lista de usuarios: {ex.Message}");
        }
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
    

