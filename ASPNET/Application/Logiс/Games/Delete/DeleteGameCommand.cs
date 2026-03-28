using ASPNET.Application.Common.Results;
using MediatR;

namespace ASPNET.Application.Login.Groups.Delete
{
    public record DeleteGameCommand(Guid gameId) : IRequest<Result>;
}
