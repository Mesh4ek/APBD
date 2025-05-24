using Test.Infrastructure.Repositories.Abstractions;
using Test.Services;
using Test.Services.Abstractions;

namespace Test.Infrastructure.Repositories.Extensions;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}