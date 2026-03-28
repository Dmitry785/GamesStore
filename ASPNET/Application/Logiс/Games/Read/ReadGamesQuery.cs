using ASPNET.Application.Common.Results;
using ASPNET.Domain.Models;
using MediatR;

namespace ASPNET.Application.Login.Games.Read
{
    public record ReadGamesQuery:IRequest<Result<List<GameInfo>>>;
}
