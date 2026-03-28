using ASPNET.Application.Common.Results;
using ASPNET.Application.Interfaces;
using ASPNET.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ASPNET.Application.Logic.Games.GetById
{
    public class GetGameByIdQueryHandler(
        IGamesDataStorage dbContext,
        ILogger<GetGameByIdQueryHandler> logger) : IRequestHandler<GetGameByIdQuery, Result<GameInfo>>
    {
        public async Task<Result<GameInfo>> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
        {
            var game = await dbContext.Games.FirstOrDefaultAsync(x => x.Id.Equals(request.gameId));
            if(game is null)
            {
                logger.LogWarning("Не удалось найти игру");
                return Result<GameInfo>.Fail("Не удалось найти игру");
            }
            return Result.Ok(game);
        }
    }
}
