using ASPNET.Application.Common.Results;
using ASPNET.Application.Interfaces;
using ASPNET.Domain.Models;
using MediatR;

namespace ASPNET.Application.Logiс.Games.Update
{
    public class UpdateGameCommandHandler(
        IGamesDataStorage dbContext) : IRequestHandler<UpdateGameCommand, Result>
    {
        public async Task<Result> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
        {
            var gameToUpdate = dbContext.Games.FirstOrDefault(x => x.Id.Equals(request.gameId));
            if (gameToUpdate is null)
                return Result.Fail("Не удалось найти");
            dbContext.Games.Remove(gameToUpdate);
            dbContext.Games.Add(new GameInfo(request.gameId, request.name, 
                request.author, request.year, request.genre, request.poster));
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }
}
