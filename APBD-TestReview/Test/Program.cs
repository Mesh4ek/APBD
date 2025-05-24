using Test.Infrastructure.Repositories.Extensions;
using Test.Middlewares;
using Test.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddApplicationServices()
    .AddInfrastructureServices()
    .AddProblemDetails();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// Use registered global error handler
app.UseExceptionHandler();
app.UseStatusCodePages();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();