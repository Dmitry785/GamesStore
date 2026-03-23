using ASPNET.Application.Interfaces;
using ASPNET.Domain.Models;

namespace ASPNET.Application.Services.Interfaces
{
    public interface IGameService
    {
        void Create(GameInfo game);
        bool Delete(Guid game);
        List<GameInfo> Read();
        bool Update(GameInfo game);
        GameInfo? GetById(Guid id);
    }
}
