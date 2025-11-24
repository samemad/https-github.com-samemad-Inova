using Inova.Application.Interfaces;
using Inova.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Inova.Application.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISpecializationService, SpecializationService>();  // ← ADD THIS
        services.AddScoped<IConsultantService, ConsultantService>();          // ← ADD THIS


        // We'll add more services later

        return services;
    }
}