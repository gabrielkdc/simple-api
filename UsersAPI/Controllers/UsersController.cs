using Microsoft.AspNetCore.Mvc;
using UsersAPI.Data;
using UsersAPI.Enums;
using UsersAPI.Models;
using UsersAPI.ServiceAbstractions;
using UsersAPI.Services.Users;

namespace UsersAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private IRegisterUserService registerUserService;
    private IUpdateUserService updateUserService;
    private IGetUserByIdService getUserByIdService;
    private GetUsersService getUserService;
    private GetUserByUsernameService getUserByUsernameService;
    private DeleteUserService deleteUserService;

    public UsersController(ApplicationDbContext context, IRegisterUserService registerUserService, IUpdateUserService updateUserService, IGetUserByIdService getUserByIdService)
    {
        _context = context;
        this.registerUserService = registerUserService;
        this.getUserByIdService = new GetUserByIdService(context);
        this.getUserByIdService = getUserByIdService;

        this.deleteUserService = new DeleteUserService(context);
        this.getUserByUsernameService = new GetUserByUsernameService(context);

        this.updateUserService = new UpdateUserService(context);
    public UsersController(ApplicationDbContext context, IRegisterUserService registerUserService, IUpdateUserService updateUserService, IGetUserByIdService getUserByIdService)
    {
        _context = context;
        this.registerUserService = registerUserService;
        this.getUserByIdService = new GetUserByIdService(context);

        this.updateUserService = new UpdateUserService(context);
    public UsersController(ApplicationDbContext context, IRegisterUserService registerUserService, IUpdateUserService updateUserService, IGetUserByIdService getUserByIdService)
    {
        _context = context;
        this.registerUserService = registerUserService;
        this.updateUserService = updateUserService;
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
    public async Task<IActionResult> GetUserById(int id)
            case ResultCode.INVALID_INPUT :
                return BadRequest("La contraseña no puede estar vacía.");
            case ResultCode.RECORDS_CONFLICT:
                return Conflict("El nombre de usuario ya est� en uso.");
            case ResultCode.SUCCESS:
                return Ok(user);
            default:
                return StatusCode(500, "Error al registrar el usuario.");
        }
    }
    
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserDetails(int id)

        var result =  await deleteUserService.DeleteUser(id);
        if (result)

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
            return Ok("Usuario eliminado exitosamente.");
        }
        
        return NotFound("Usuario no encontrado.");
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

        var getUserResult = await getUserByUsernameService.GetUserByUsername(username);

        if (getUserResult != null && getUserResult.Username != null)
        {
            return Ok(getUserResult);
        }
        else
        {
            return NotFound("El usuario no encontrado");
        }     
    }
}
    

