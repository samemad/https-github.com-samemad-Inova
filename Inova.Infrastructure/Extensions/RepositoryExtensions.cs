using Inova.Domain.Repositories;
using Inova.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Inova.Infrastructure.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IConsultantRepository, ConsultantRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ISpecializationRepository, SpecializationRepository>();

        // We'll add more later as needed

        return services;
    }
}