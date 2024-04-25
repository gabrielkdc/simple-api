using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;
using UsersAPI.Repositorios;
using UsersAPI.RepositoryAbstractions;
using UsersAPI.ServiceAbstractions;
using UsersAPI.Services;
using UsersAPI.Services.Users;
using UsersAPI.Services.Authentification;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRegisterUserService, RegisterUserService>();
builder.Services.AddScoped<IUpdateUserService, UpdateUserService>();
builder.Services.AddScoped<IGetUserByIdService, GetUserByIdService>();
builder.Services.AddScoped<IGetUserByUsernameService, GetUserByUsernameService>();
builder.Services.AddScoped<IGetUsersService, GetUsersService>();
builder.Services.AddScoped<IDeleteUserService, DeleteUserService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();    



var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
