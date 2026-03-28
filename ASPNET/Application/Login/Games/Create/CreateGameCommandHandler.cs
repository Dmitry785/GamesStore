using System.Text.RegularExpressions;
using ASPNET.Application.Common.Results;
using ASPNET.Application.Interfaces;
using ASPNET.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ASPNET.Application.Login.Groups.Create
{
    public class CreateGameCommandHandler(
    IGamesDataStorage dbContext,
    ILogger<CreateGameCommandHandler> logger) : IRequestHandler<CreateGameCommand, Result>
    {
        public async Task<Result> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            if (request.year <= 1980)
            {
                string errorMessage = $"Игра [{request.name}]. Недопустимая дата релиза [{request.year}]";
                logger.LogWarning(errorMessage);
                return Result<Guid>.Fail(errorMessage);
            }

            var game = new GameInfo(Guid.NewGuid(), request.name, request.author, request.year, request.genre, request.poster);

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }
}
