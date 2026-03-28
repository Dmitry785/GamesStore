using ASPNET.Application.Common.Results;
using ASPNET.Domain.Models;
using MediatR;

namespace ASPNET.Application.Logic.Games.Create
{
    public record CreateGameCommand(string name, string author,
        int year, string genre, string? poster) : IRequest<Result>
    {
        public CreateGameCommand(GameInfo game)
            : this(game.Name, game.Author, game.ReleaseYear, game.Genre, game.Poster) { }
    }
}
