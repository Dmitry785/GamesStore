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
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                WebRootPath = "StaticFiles"
            });
            var gamesDataStorage = new FileGamesDataStorage("data.txt");
            gamesDataStorage.Initialize();
            gamesDataStorage.SetDefaultGamesIfEmpty();

            builder.Services.AddSingleton<IGamesDataStorage>(gamesDataStorage);
            builder.Services.AddTransient<IGameService, GameService>();

            var app = builder.Build();

            app.MapGet("/games", (string? genre, string? author, IGameService adapter) =>
            {
                var filteredGames = adapter.Read().Where(x =>
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
