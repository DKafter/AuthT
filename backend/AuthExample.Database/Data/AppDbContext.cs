using AuthExample.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthExample.Database.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public AppDbContext()
        {}
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
