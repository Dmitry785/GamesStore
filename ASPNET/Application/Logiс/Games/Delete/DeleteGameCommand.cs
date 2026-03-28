using ASPNET.Application.Common.Results;
using MediatR;

namespace ASPNET.Application.Logic.Games.Delete
{
    public record DeleteGameCommand(Guid gameId) : IRequest<Result>;
}
