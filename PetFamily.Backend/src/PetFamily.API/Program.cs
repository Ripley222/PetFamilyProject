using Framework;
using Microsoft.EntityFrameworkCore;
using PetFamily.API.Extensions;
using Serilog;
using Serilog.Events;
using Species.Database;
using Species.Presenters;
using Volunteers.Infrastructure.Postgres;
using Volunteers.Presenters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq")
                 ?? throw new ArgumentNullException(null, "Seq connection string not found."))
    .Enrich.WithThreadId()
    .Enrich.WithEnvironmentName()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentUserName()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSerilog();

builder.Services
    .AddVolunteerModule(builder.Configuration)
    .AddSpeciesModule();

var app = builder.Build();

app.UseExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await using var scope = app.Services.CreateAsyncScope();
    var volunteersDbContext = scope.ServiceProvider.GetRequiredService<VolunteerDbContext>();
    var speciesDbContext = scope.ServiceProvider.GetRequiredService<SpeciesDbContext>();

    await volunteersDbContext.Database.MigrateAsync();
    await speciesDbContext.Database.MigrateAsync();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapControllers();

app.MapEndpoints();

app.Run();

namespace PetFamily.API
{
    public partial class Program
    {
    }
}