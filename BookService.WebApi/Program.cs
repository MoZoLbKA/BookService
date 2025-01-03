using BookService.Application;
using BookService.Infrastructure.Persistence;
using BookService.Infrastructure.Persistence.Contexts;
using BookService.Infrastructure.Persistence.Seeds;
using BookService.JwtAuth;
using BookService.WebApi.Infrastructure.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

bool useInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");

builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfrastructure(builder.Configuration, useInMemoryDatabase);
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
JsonSerializerOptions options = new JsonSerializerOptions()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    PropertyNameCaseInsensitive = true,
    WriteIndented = true,
};
builder.Services.AddSingleton(options);
SwaggerExtensions.AddSwaggerWithVersioning(builder);
builder.Services.AddAnyCors();
builder.Services.AddHealthChecks();
JWTConfigurator.Configure(builder);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    if (!useInMemoryDatabase)
    {
        await services.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
    }

    await DefaultData.SeedAsync(services.GetRequiredService<ApplicationDbContext>());
}
JWTConfigurator.Configure(app);
app.UseAnyCors();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerWithVersioning();
app.UseHealthChecks("/health");
app.MapControllers();
app.UseSerilogRequestLogging();

app.Run();

