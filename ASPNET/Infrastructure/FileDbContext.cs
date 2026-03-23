using ASPNET.Application.Interfaces;
using ASPNET.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ASPNET.Infrastructure
{
    public class FileGamesDataStorage : DbContext, IGamesDataStorage
    {
        private readonly string _path;
        public DbSet<GameInfo> Games { get; set; }
        public FileGamesDataStorage(string path)
        {
            _path = path;
        }
        public void Initialize()
        {
            var games = LoadFromFile() ?? new List<GameInfo>();
            Games.AddRange(games);
            SaveChangesAsync(default).Wait();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Games");
        }
        private List<GameInfo>? LoadFromFile()
        {
            try
            {
                using (FileStream fs = new FileStream(_path, FileMode.Open))
                using (StreamReader sr = new StreamReader(fs))
                {
                    var result = new List<GameInfo>();
                    var parseString = sr.ReadToEnd();
                    var games = JsonSerializer.Deserialize<List<GameInfo>>(parseString);
                    return games;
                }
            }
            catch { }
            return null;
        }
        public new async Task SaveChangesAsync(CancellationToken token)
        {
            if (!ChangeTracker.HasChanges())
                return;
            try
            {
                using (FileStream fs = new FileStream(_path, FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    await sw.WriteAsync(JsonSerializer.Serialize(Games.Local.ToList()));
                }
                await base.SaveChangesAsync(token);
            }
            catch { }
        }
    }
}
