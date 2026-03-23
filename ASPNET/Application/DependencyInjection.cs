using ASPNET.Application.Interfaces;
using ASPNET.Application.Services;
using ASPNET.Application.Services.Interfaces;
using ASPNET.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ASPNET
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddScoped<IGameService, GameService>();
            return services;
        }
    }
}
