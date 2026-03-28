using ASPNET.Application.Common.Results;
using ASPNET.Domain.Models;
using MediatR;

namespace ASPNET.Application.Logiс.Games.Update
{
    public record UpdateGameCommand(Guid gameId, string name, string author,
        int year, string genre, string? poster) : IRequest<Result>
    {
        public UpdateGameCommand(GameInfo game)
            : this(game.Id, game.Name, game.Author, game.ReleaseYear, game.Genre, game.Poster) { }
    }
}
