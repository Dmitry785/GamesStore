using ASPNET.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNET.Application.Interfaces
{
    public interface IGamesDataStorage
    {
        DbSet<GameInfo> Games { get; set; }
        Task SaveChangesAsync(CancellationToken token);
    }
}
