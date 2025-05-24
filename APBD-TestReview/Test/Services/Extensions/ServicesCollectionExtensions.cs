using Test.Infrastructure.Repositories.Abstractions;
using Test.Services.Abstractions;

namespace Test.Services.Extensions;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITeamMemberService, TeamMemberService>();
        services.AddScoped<ITaskService, TaskService>();
        
        return services;
    }
}