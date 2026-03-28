using ASPNET.Application.Common.Results;
using ASPNET.Application.Interfaces;
using MediatR;

namespace ASPNET.Application.Logic.Games.Delete
{
    public class DeleteGameCommandHandler(
    IGamesDataStorage dbContext,
    ILogger<DeleteGameCommandHandler> logger) : IRequestHandler<DeleteGameCommand, Result>
    {
        public async Task<Result> Handle(DeleteGameCommand request, CancellationToken cancellationToken)
        {
            var game = dbContext.Games.FirstOrDefault(g => g.Id.Equals(request.gameId));
            if (game is null)
            {
                logger.LogWarning("Не удалось найти игру");
                return Result.Fail("Не удалось найти игру");
            }
            dbContext.Games.Remove(game);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }
}
