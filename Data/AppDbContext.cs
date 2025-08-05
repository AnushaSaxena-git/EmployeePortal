using EmployeePortal.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortal.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions Options) : base(Options)
        {


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", Password = "admin123" }
            );
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Hobbies> Hobbies { get; set; }

        public DbSet<User> users { get; set; }  
    }
}
