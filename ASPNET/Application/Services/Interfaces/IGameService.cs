using ASPNET.Application.Interfaces;
using ASPNET.Domain.Models;

namespace ASPNET.Application.Services.Interfaces
{
    public interface IGameService
    {
        Task Create(GameInfo game);
        Task<bool> Delete(Guid game);
        Task<List<GameInfo>> Read();
        Task<bool> Update(GameInfo game);
        Task<GameInfo?> GetById(Guid id);
    }
}
