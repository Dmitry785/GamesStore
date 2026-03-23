using ASPNET.Application.Interfaces;
using ASPNET.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNET.Infrastructure
{
    public class AppDbContext : DbContext, IGamesDataStorage
    {
        public DbSet<GameInfo> Games { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public new async Task SaveChangesAsync(CancellationToken token)
        {
            await SaveChangesAsync(token);
        }
    }
}
