using ASPNET.Application.Common.Results;
using ASPNET.Application.Interfaces;
using ASPNET.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ASPNET.Application.Login.Games.GetById
{
    public class GetGameByIdQueryHandler(
        IGamesDataStorage dbContext) : IRequestHandler<GetGameByIdQuery, Result<GameInfo>>
    {
        public async Task<Result<GameInfo>> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
        {
            var game = await dbContext.Games.FirstOrDefaultAsync(x => x.Id.Equals(request.gameId));
            return game is null ? Result<GameInfo>.Fail("Не удалось найти игру") : Result.Ok(game);
        }
    }
}
