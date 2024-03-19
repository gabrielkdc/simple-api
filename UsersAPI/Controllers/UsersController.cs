using Microsoft.AspNetCore.Mvc;
using UsersAPI.Data;
using UsersAPI.Models;

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
        if (ModelState.IsValid)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("Usuario registrado exitosamente.");
        }
        return BadRequest("Datos de usuario no válidos.");
    }
}
