using autoweb.Models;
using Microsoft.EntityFrameworkCore;

namespace autoweb.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<Brand> Brand { get; set; }
    }
}
