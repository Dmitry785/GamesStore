using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using ASPNET;
using ASPNET.Application.Interfaces;
using ASPNET.Application.Logic.Games.Models;
using ASPNET.Application.Logic.Games.Read;
using ASPNET.Domain.Models;
using ASPNET.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable CS0162

namespace WebApplication1
{
    public class Program
    {
        const bool USE_FILE_DS = false;
        const bool USE_PARAMS_IN_BODY = false;
        public static void Main(string[] args)
        {
            var defaultGames = new List<GameInfo>()
                 {
                     new GameInfo(Guid.NewGuid(), "Far Cry 3", "Ubisoft", 2012, "Action Adventure RPG",
                     "https://upload.wikimedia.org/wikipedia/ru/thumb/a/a0/Far_Cry_3_Box_Art_PC.jpeg/330px-Far_Cry_3_Box_Art_PC.jpeg"),
                     new GameInfo(Guid.NewGuid(), "Minecraft", "Mojang", 2009, "Adventure Sandbox",
                     "https://image.api.playstation.com/vulcan/ap/rnd/202407/0401/165b1afdf14c26321af90746e4a42e23416bb74eafd34b0c.png"),
                     new GameInfo(Guid.NewGuid(), "Counter Strike: Source", "Valve", 2004, "Action",
                     "https://upload.wikimedia.org/wikipedia/ru/a/a9/CS-S_Box.jpg"),
                     new GameInfo(Guid.NewGuid(), "Grand Theft Autho: San Andreas", "Rockstar", 2004, "Action Adventure",
                     "https://upload.wikimedia.org/wikipedia/ru/thumb/7/75/Grand_Theft_Auto_San_Andreas.jpg/960px-Grand_Theft_Auto_San_Andreas.jpg"),
                     new GameInfo(Guid.NewGuid(), "Mafia 2", "Czech", 2010, "Action Adventure",
                     "https://upload.wikimedia.org/wikipedia/en/0/0d/Mafia_II_Boxart.jpg")
                 };
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                WebRootPath = "StaticFiles"
            });
            //задание 3
            if(USE_FILE_DS)
                builder.Services.AddFileDataStorage();
            else
                builder.Services.AddSqlDataStorage(builder.Configuration);
            builder.Services.AddApplicationLayer();

            var app = builder.Build();

            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            var scope = scopeFactory.CreateScope();
            var dataStorage = scope.ServiceProvider.GetRequiredService<IGamesDataStorage>();
            if (dataStorage.Games.Count() == 0)
            {
                dataStorage.Games.AddRange(defaultGames);
                dataStorage.SaveChangesAsync(default);
            }

            app.MapGet("/games", async (string? genre, string? author, IMediator mediator) =>
            {
                var result = await mediator.Send(new ReadGamesQuery());
                if (result.Failed)
                    return Results.BadRequest();

                var filteredGames = result.Data!.Where(x =>
                    (genre is null ? true : x.Genre.ToLower().Contains(genre.ToLower())) &&
                    (author is null ? true : x.Author.ToLower().Contains(author.ToLower()))
                );
                return Results.Json(filteredGames);
            });
            if(USE_PARAMS_IN_BODY)
                app.AddDatedRouting();
            else
                app.AddAdvancedRouting();
            app.Run();
        }
    }
}
