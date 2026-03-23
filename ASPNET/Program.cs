using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using ASPNET;
using ASPNET.Application.Interfaces;
using ASPNET.Application.Services;
using ASPNET.Application.Services.Interfaces;
using ASPNET.Domain.Models;
using ASPNET.Infrastructure;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class Program
    {
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
            //builder.Services.AddFileDataStorage();
            //builder.Services.AddSqlDataStorage(builder.Configuration);
            builder.Services.AddSqlDataStorage();
            builder.Services.AddApplicationLayer();

            var app = builder.Build();

            var dataStorage = app.Services.GetRequiredService<IGamesDataStorage>();
            if (dataStorage.Games.Count() == 0)
            {
                dataStorage.Games.AddRange(defaultGames);
                dataStorage.SaveChangesAsync(default);
            }

            app.MapGet("/games", async (string? genre, string? author, IGameService adapter) =>
            {
                var filteredGames = (await adapter.Read()).Where(x =>
                    (genre is null ? true : x.Genre.ToLower().Contains(genre.ToLower())) &&
                    (author is null ? true : x.Author.ToLower().Contains(author.ToLower()))
                ).Select(x => new GameInfo{ Name = x.Name, Poster = x.Poster,
                    Genre = x.Genre, Author = x.Author, Id = x.Id });
                return Results.Json(filteredGames);
            });
            //для 2 задания
            //добавлены staticfiles и json отправка
            app.AddAdvancedRouting();

            //для 1 задания
            //отправка в теле запроса
            //app.AddDatedRouting();
            app.Run();
        }
    }
}
