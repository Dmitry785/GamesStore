using ASPNET.Application.Common.Results;
using ASPNET.Application.Logic.Games.Models;
using ASPNET.Domain.Models;
using MediatR;

namespace ASPNET.Application.Logic.Games.Read
{
    public record ReadGamesQuery:IRequest<Result<List<ShortGameInfo>>>;
}
