using ASPNET.Application.Services;
using ASPNET.Application.Services.Interfaces;
using ASPNET.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET
{
    public static class Endpoints
    {
        public static WebApplication AddAdvancedRouting(this WebApplication app)
        {
            var defaultFileOptions = new DefaultFilesOptions();
            defaultFileOptions.DefaultFileNames.Add("startpage.html");

            app.UseDefaultFiles(defaultFileOptions);
            app.UseStaticFiles();


            app.MapGet("game/{id:guid}", async (Guid id, IGameService service) =>
            {
                var game = await service.GetById(id);
                if (game is null)
                    return Results.NotFound();
                return Results.Json(game);
            });
            app.MapPost("/game", async ([FromBody] GameInfo game, IGameService service) =>
            {
                await service.Create(game);
                return Results.Ok();
            });
            app.MapPut("/game", async ([FromBody] GameInfo game, IGameService service) =>
            {
                if (!await service.Update(game))
                    return Results.NotFound();
                return Results.Ok();
            });
            app.MapDelete("/game", async ([FromBody] Guid id, IGameService service) =>
            {
                if (!await service.Delete(id))
                    return Results.NotFound();
                return Results.Ok();
            });
            return app;
        }
        public static WebApplication AddDatedRouting(this WebApplication app)
        {
            app.MapGet("/game/{id:guid}", async (Guid id, IGameService adapter) =>
            {
                var game = await adapter.GetById(id);
                if (game is null)
                    return Results.NotFound();
                return Results.Json(game);
            });
            app.MapDelete("/game/{id:guid}", async (Guid id, IGameService adapter) =>
            {
                if (await adapter.Delete(id))
                    return Results.Ok();
                return Results.NotFound();
            });
            app.MapPost("/game/{name}/{author}/{release:int}/{genre}/{poster?}", async (string name, string author,
                int release, string genre, string? poster, IGameService adapter) =>
            {
                if (poster is not null)
                    poster = Uri.UnescapeDataString(poster);
                await adapter.Create(new GameInfo(Guid.NewGuid(), name, author, release, genre, poster));
                return Results.Ok();
            });

            app.MapPut("/game/{id:guid}/{name}/{author}/{release:int}/{genre}/{poster?}", async (Guid id, string name, string author,
                int release, string genre, string? poster, IGameService adapter) =>
            {
                if (poster is not null)
                    poster = Uri.UnescapeDataString(poster);
                if (await adapter.Update(new GameInfo(id, name, author, release, genre, poster)))
                {
                    return Results.Ok();
                }
                return Results.NotFound();
            });
            app.MapGet("/", async (context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("startpage.html");
            });
            return app;
        }
    }
}
