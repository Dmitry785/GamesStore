using ASPNET.Application.Logic.Games.GetById;
using ASPNET.Application.Logic.Games.Create;
using ASPNET.Application.Logic.Games.Delete;
using ASPNET.Application.Logiс.Games.Update;
using ASPNET.Domain.Models;
using MediatR;
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


            app.MapGet("game/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetGameByIdQuery(id));
                if (result.Failed)
                    return Results.NotFound();
                return Results.Json(result.Data!);
            });
            _ = app.MapPost("/game", async ([FromBody] GameInfo game, IMediator mediator) =>
            {
                await mediator.Send(new CreateGameCommand(game));
                return Results.Ok();
            });
            app.MapPut("/game", async ([FromBody] GameInfo game, IMediator mediator) =>
            {
                var result = await mediator.Send(new UpdateGameCommand(game));
                if(result.Failed)
                    return Results.NotFound();
                return Results.Ok();
            });
            app.MapDelete("/game", async ([FromBody] Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new DeleteGameCommand(id));
                if(result.Failed)
                    return Results.NotFound();
                return Results.Ok();
            });
            return app;
        }
        public static WebApplication AddDatedRouting(this WebApplication app)
        {
            app.MapGet("/game/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetGameByIdQuery(id));
                if (result.Failed)
                    return Results.NotFound();
                return Results.Json(result.Data!);
            });
            app.MapDelete("/game/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new DeleteGameCommand(id));
                if (result.Failed)
                    return Results.NotFound();
                return Results.Ok();
            });
            app.MapPost("/game/{name}/{author}/{release:int}/{genre}/{poster?}", async (string name, string author,
                int release, string genre, string? poster, IMediator mediator) =>
            {
                if (poster is not null)
                    poster = Uri.UnescapeDataString(poster);
                var result = await mediator.Send(new CreateGameCommand(name, author, release, genre, poster));
                if (result.Failed)
                    return Results.BadRequest();
                return Results.Ok();
            });

            app.MapPut("/game/{id:guid}/{name}/{author}/{release:int}/{genre}/{poster?}", async (Guid id, string name, string author,
                int release, string genre, string? poster, IMediator mediator) =>
            {
                if (poster is not null)
                    poster = Uri.UnescapeDataString(poster);
                var result = await mediator.Send(new UpdateGameCommand(id, name, author, release, genre, poster));
                if (result.Failed)
                    return Results.NotFound();
                return Results.Ok();
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
