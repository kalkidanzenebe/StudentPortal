using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Models.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // If there is a conflict, use the fully qualified name
    public DbSet<StudentPortal.Web.Models.Entities.Student> Students { get; set; }
    public DbSet<User> Users { get; set; }
}
