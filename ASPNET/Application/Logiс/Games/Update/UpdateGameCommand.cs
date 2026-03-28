using ASPNET.Application.Common.Results;
using MediatR;

namespace ASPNET.Application.Logiс.Games.Update
{
    public record UpdateGameCommand(Guid gameId, string name, string author,
        int year, string genre, string? poster) : IRequest<Result>;
}
