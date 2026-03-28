using ASPNET.Application.Common.Results;
using ASPNET.Domain.Models;
using MediatR;

namespace ASPNET.Application.Login.Games.GetById
{
    public record GetGameByIdQuery(Guid id) : IRequest<Result<GameInfo>>;
}
