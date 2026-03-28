using ASPNET.Application.Common.Results;
using ASPNET.Application.Interfaces;
using ASPNET.Application.Logic.Games.Models;
using ASPNET.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ASPNET.Application.Logic.Games.Read
{
    public class ReadGamesQueryHandler(IGamesDataStorage dbContext,
        ILogger<ReadGamesQueryHandler> logger) : IRequestHandler<ReadGamesQuery, Result<List<ShortGameInfo>>>
    {
        public async Task<Result<List<ShortGameInfo>>> Handle(ReadGamesQuery request, CancellationToken cancellationToken)
        {
            return Result.Ok(dbContext.Games.Select(x => 
                new ShortGameInfo()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Author = x.Author,
                    Poster = x.Poster,
                    Genre = x.Genre
                }).ToList()
            );
        }
    }
}
