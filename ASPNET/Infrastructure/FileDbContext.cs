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
        public void SetDefaultGamesIfEmpty()
        {
            if (Games.ToList().Count == 0)
            {
                //добавляем значения по дефолту
                var games = new List<GameInfo>()
                 {
                     new GameInfo(Guid.NewGuid(), "Far Cry 3", "Ubisoft", 2012, "Action Adventure RPG",
                     "https://upload.wikimedia.org/wikipedia/ru/thumb/a/a0/Far_Cry_3_Box_Art_PC.jpeg/330px-Far_Cry_3_Box_Art_PC.jpeg"),
                     new GameInfo(Guid.NewGuid(), "Minecraft", "Mojang", 2009, "Adventure Sandbox",
                     "https://image.api.playstation.com/vulcan/ap/rnd/202407/0401/165b1afdf14c26321af90746e4a42e23416bb74eafd34b0c.png"),
                     new GameInfo(Guid.NewGuid(), "Counter Strike: Source", "Valve", 2004, "Action",
                     "https://upload.wikimedia.org/wikipedia/ru/a/a9/CS-S_Box.jpg"),
                     new GameInfo(Guid.NewGuid(), "Grand Theft Autho: San Andreas", "Rockstar", 2004, "Action Adventure",
                     "https://upload.wikimedia.org/wikipedia/ru/thumb/7/75/Grand_Theft_Auto_San_Andreas.jpg/960px-Grand_Theft_Auto_San_Andreas.jpg"),
                     new GameInfo(Guid.NewGuid(), "Mafia 2", "Czech", 2010, "Action Adventure",
                     "https://upload.wikimedia.org/wikipedia/en/0/0d/Mafia_II_Boxart.jpg")
                 };;
                Games.AddRange(games);
                SaveChangesAsync(default).Wait();
                return;
            }
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
