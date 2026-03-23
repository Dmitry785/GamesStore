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


            app.MapGet("game/{id:guid}", (Guid id, GamesDataStorageService service) =>
            {
                Console.WriteLine("get");
                var game = service.Read().FirstOrDefault(x => x.Id.Equals(id));
                if (game is null)
                    return Results.NotFound();
                return Results.Json(game);
            });
            app.MapPost("/game", ([FromBody] GameInfo game, GamesDataStorageService service) =>
            {
                Console.WriteLine("post");
                service.Create(game);
                return Results.Ok();
            });
            app.MapPut("/game", ([FromBody] GameInfo game, GamesDataStorageService service) =>
            {
                Console.WriteLine("put");
                if (!service.Update(game))
                    return Results.NotFound();
                return Results.Ok();
            });
            app.MapDelete("/game", ([FromBody] Guid id, GamesDataStorageService service) =>
            {
                Console.WriteLine($"delete {id}");
                var game = service.Read().FirstOrDefault(x => x.Id.Equals(id));
                if (game is null)
                    return Results.NotFound();
                if (!service.Delete(game))
                    return Results.NotFound();
                return Results.Ok();
            });
            return app;
        }
        public static WebApplication AddDatedRouting(this WebApplication app)
        {
            app.MapGet("/game/{id:guid}", (Guid id, GamesDataStorageService adapter) =>
            {
                var game = adapter.Read().FirstOrDefault(x => x.Id.Equals(id));
                if (game is null)
                    return Results.NotFound();
                return Results.Json(game);
            });
            app.MapDelete("/game/{id:guid}", (Guid id, GamesDataStorageService adapter) =>
            {
                var game = adapter.Read().FirstOrDefault(x => x.Id.Equals(id));
                if (game is not null && adapter.Delete(game))
                    return Results.Ok();
                return Results.NotFound();
            });
            app.MapPost("/game/{name}/{author}/{release:int}/{genre}/{poster?}", (string name, string author,
                int release, string genre, string? poster, GamesDataStorageService adapter) =>
            {
                if (poster is not null)
                    poster = Uri.UnescapeDataString(poster);
                adapter.Create(new GameInfo(Guid.NewGuid(), name, author, release, genre, poster));
                return Results.Ok();
            });

            app.MapPut("/game/{id:guid}/{name}/{author}/{release:int}/{genre}/{poster?}", (Guid id, string name, string author,
                int release, string genre, string? poster, GamesDataStorageService adapter) =>
            {
                if (poster is not null)
                    poster = Uri.UnescapeDataString(poster);
                if (adapter.Update(new GameInfo(id, name, author, release, genre, poster)))
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
