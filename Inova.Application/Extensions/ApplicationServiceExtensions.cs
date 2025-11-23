using Inova.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Inova.Application.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        // We'll add more services later

        return services;
    }
}