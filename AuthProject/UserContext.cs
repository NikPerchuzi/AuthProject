using AuthProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthProject
{

    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

