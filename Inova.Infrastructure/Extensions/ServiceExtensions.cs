using Inova.Application.Interfaces;
using Inova.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Inova.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Register Email Service
        services.AddScoped<IEmailService, EmailService>();

        // Register Token Service
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}