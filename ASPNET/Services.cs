using System.Text.Json;
using WebApplication1;

namespace ASPNET
{
    public interface IGamesDataStorage
    {
        void SaveChanges();
        List<GameInfo> Games { get; set; }
    }
    public class FileGamesDataStorage : IGamesDataStorage
    {
        private readonly string _path;
        private List<GameInfo> _loadedGames;
        private List<GameInfo> _changes;
        public FileGamesDataStorage(string path)
        {
            _path = path;
            var games = LoadFromFile();
            if (games is null)
            {
                _changes = new List<GameInfo>();
                _loadedGames = new List<GameInfo>();
            }
            else
            {
                _changes = games.ToList(); ;
                _loadedGames = games.ToList();
            }
        }
        public void SetDefaultGamesIfEmpty()
        {
            if (Games.Count == 0)
            {
                //добавляем значения по дефолту
                Games = new List<GameInfo>()
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
                 };
                SaveChanges();
            }
        }
        public List<GameInfo> Games
        {
            get => _changes;
            set
            {
                _changes = value;
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
        private bool IsChanged()
        {
            if (_changes.Count != _loadedGames.Count)
                return true;
            foreach (GameInfo game in _changes)
            {
                if (!_loadedGames.Exists(x => x.Equals(game)))
                    return true;
            }
            return false;
        }
        public void SaveChanges()
        {
            if (!IsChanged())
                return;
            try
            {
                using (FileStream fs = new FileStream(_path, FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(JsonSerializer.Serialize(_changes));
                }
                _loadedGames = _changes.ToList();
            }
            catch { }
        }
    }
    public class GamesDataStorageService
    {
        private IGamesDataStorage _dataStorage;
        public GamesDataStorageService(IGamesDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }
        public void Create(GameInfo game)
        {
            game.Id = Guid.NewGuid();
            _dataStorage.Games.Add(game);
            _dataStorage.SaveChanges();
        }
        public bool Delete(GameInfo game)
        {
            var i = _dataStorage.Games.RemoveAll(g => g.Equals(game));
            _dataStorage.SaveChanges();
            return i > 0;
        }
        public List<GameInfo> Read()
        {
            return _dataStorage.Games;
        }
        public bool Update(GameInfo game)
        {
            if (_dataStorage.Games.RemoveAll(x => x.Id.Equals(game.Id)) > 0)
            {
                _dataStorage.Games.Add(game);
                _dataStorage.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
