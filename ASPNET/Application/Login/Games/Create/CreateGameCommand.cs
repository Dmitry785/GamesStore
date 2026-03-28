using ASPNET.Application.Common.Results;
using MediatR;

namespace ASPNET.Application.Login.Groups.Create
{
    public record CreateGameCommand(string name, string author, 
        int year, string genre, string? poster) : IRequest<Result>;
}
