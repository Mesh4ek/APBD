using Task9.Application.Services;
using Task9.Application.Services.Interfaces;

namespace Task9.Application;

public static class ApplicationServicesExtension
{
    public static void RegisterApplicationServices(this IServiceCollection app)
    {
        app.AddScoped<ITripService, TripService>();
        app.AddScoped<IClientService, ClientService>();
        app.AddScoped<IClientTripService, ClientTripService>();
    }
}