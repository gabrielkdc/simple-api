using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UsersAPI.Models;

namespace UsersAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }

}
