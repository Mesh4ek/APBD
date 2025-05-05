using Task7.Middlewares;
using Task7.Repositories.Implementations;
using Task7.Repositories.Interfaces;
using Task7.Services.Implementations;
using Task7.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Repositories
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<IClientTripRepository, ClientTripRepository>();
builder.Services.AddScoped<ICountryTripRepository, CountryTripRepository>();

// Services
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ITripService, TripService>();

var app = builder.Build();

// Global exception handler
app.UseMiddleware<GlobalExceptionHandler>();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();