using FastEndpoints;
using FastEndpoints.Swagger;
using FastEndpointsTemplate.Api.Middlewares;
using FastEndpointsTemplate.Application;
using FastEndpointsTemplate.Infrastructure;
using FastEndpointsTemplate.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Logging Estructurado con Serilog
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

// Registrar capas de la Arquitectura
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// FastEndpoints & Swagger OpenAPI
builder.Services.AddFastEndpoints(o =>
{
    o.Assemblies = [typeof(Program).Assembly];
});
builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.Title = "FastEndpoints .NET 10 API Template";
        s.Version = "v1";
        s.Description = "Template de Arquitectura Empresarial con FastEndpoints y .NET 10";
    };
    o.ShortSchemaNames = true;
});

// Manejo global de excepciones
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// Inicialización de Base de Datos para desarrollo
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.UseExceptionHandler();
app.UseSerilogRequestLogging();

app.UseFastEndpoints(c =>
{
    c.Errors.UseProblemDetails();
});

app.UseSwaggerGen();

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.MapHealthChecks("/health");

app.Run();

// Necesario para pruebas de integración con WebApplicationFactory
public partial class Program { }
