using Task9.Application.Repository;
using Task9.Infrastructure.Repository;

namespace Task9.Infrastructure;

public static class InfrastructureServicesExtension
{
    public static void RegisterInfraServices(this IServiceCollection app)
    {
        app.AddScoped<ITripRepository, TripRepository>();
        app.AddScoped<IClientRepository, ClientRepository>();
        app.AddDbContext<Task9Context>();
        app.AddScoped<IClientTripRepository, ClientTripRepository>();
    }
}