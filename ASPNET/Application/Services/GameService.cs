using ASPNET.Application.Interfaces;
using ASPNET.Application.Services.Interfaces;
using ASPNET.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNET.Application.Services
{
    public class GameService : IGameService
    {
        private IGamesDataStorage _dataStorage;
        public GameService(IGamesDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }
        public async Task Create(GameInfo game)
        {
            game.Id = Guid.NewGuid();
            _dataStorage.Games.Add(game);
            await _dataStorage.SaveChangesAsync(default);
        }
        public async Task<bool> Delete(Guid gameId)
        {
            var game = _dataStorage.Games.FirstOrDefault(g => g.Id.Equals(gameId));
            if (game is null)
                return false;
            _dataStorage.Games.Remove(game);
            await _dataStorage.SaveChangesAsync(default);
            return true;
        }

        public async Task<GameInfo?> GetById(Guid id)
        {
            return await _dataStorage.Games.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<List<GameInfo>> Read()
        {
            return await _dataStorage.Games.ToListAsync();
        }
        public async Task<bool> Update(GameInfo game)
        {
            var gameToUpdate = _dataStorage.Games.FirstOrDefault(x => x.Id.Equals(game.Id));
            if (gameToUpdate is null)
                return false;
            _dataStorage.Games.Remove(gameToUpdate);
            _dataStorage.Games.Add(game);
            await _dataStorage.SaveChangesAsync(default);
            return true;
        }
    }
}
