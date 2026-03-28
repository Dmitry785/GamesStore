using ASPNET.Application.Interfaces;
using ASPNET.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNET.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFileDataStorage(this IServiceCollection services)
        {
            var gamesDataStorage = new FileGamesDataStorage("data.txt");
            gamesDataStorage.Initialize();

            services.AddScoped<IGamesDataStorage>(provider => gamesDataStorage);
            return services;
        }
        public static IServiceCollection AddSqlDataStorage(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Games");
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectionString));
            services.AddScoped<IGamesDataStorage>(provider => provider.GetRequiredService<AppDbContext>());
            return services;
        }
    }
}
