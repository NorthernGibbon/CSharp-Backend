using JacobCorp.Models;
using Microsoft.EntityFrameworkCore;

namespace JacobCorp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        //Andra db sets
    }
}
