using ASPNET.Application.Common.Results;
using ASPNET.Application.Interfaces;
using ASPNET.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ASPNET.Application.Logic.Games.Read
{
    public class ReadGamesQueryHandler(IGamesDataStorage dbContext,
        ILogger<ReadGamesQueryHandler> logger) : IRequestHandler<ReadGamesQuery, Result<List<GameInfo>>>
    {
        public async Task<Result<List<GameInfo>>> Handle(ReadGamesQuery request, CancellationToken cancellationToken)
        {
            return Result.Ok(await dbContext.Games.ToListAsync());
        }
    }
}
