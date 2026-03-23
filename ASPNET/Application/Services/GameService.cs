using ASPNET.Application.Interfaces;
using ASPNET.Application.Services.Interfaces;
using ASPNET.Domain.Models;

namespace ASPNET.Application.Services
{
    public class GameService : IGameService
    {
        private IGamesDataStorage _dataStorage;
        public GameService(IGamesDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }
        public void Create(GameInfo game)
        {
            game.Id = Guid.NewGuid();
            _dataStorage.Games.Add(game);
            _dataStorage.SaveChangesAsync(default);
        }
        public bool Delete(Guid gameId)
        {
            var game = _dataStorage.Games.FirstOrDefault(g => g.Id.Equals(gameId));
            if (game is null)
                return false;
            _dataStorage.Games.Remove(game);
            _dataStorage.SaveChangesAsync(default);
            return true;
        }

        public GameInfo? GetById(Guid id)
        {
            return _dataStorage.Games.FirstOrDefault(x => x.Id.Equals(id));
        }

        public List<GameInfo> Read()
        {
            return _dataStorage.Games.ToList();
        }
        public bool Update(GameInfo game)
        {
            var gameToUpdate = _dataStorage.Games.FirstOrDefault(x => x.Id.Equals(game.Id));
            if (gameToUpdate is null)
                return false;
            _dataStorage.Games.Remove(gameToUpdate);
            _dataStorage.Games.Add(game);
            _dataStorage.SaveChangesAsync(default);
            return true;
        }
    }
}
