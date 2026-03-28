using ASPNET.Application.Interfaces;
using ASPNET.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ASPNET
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            return services;
        }
    }
}
