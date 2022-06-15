using Microsoft.EntityFrameworkCore;
using MyAngularApp.Entities;

namespace MyAngularApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options):base (options)
        {
        
        }

        public DbSet<AppUser> Users { get; set; }
    }
}
