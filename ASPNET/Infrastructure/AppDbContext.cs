using ASPNET.Application.Interfaces;
using ASPNET.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNET.Infrastructure
{
    public class AppDbContext : DbContext, IGamesDataStorage
    {
        public DbSet<GameInfo> Games { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public AppDbContext()
        {
            Database.EnsureCreated();
        }
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Games;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }*/
        public new async Task SaveChangesAsync(CancellationToken token)
        {
            await base.SaveChangesAsync(token);
        }
    }
}
