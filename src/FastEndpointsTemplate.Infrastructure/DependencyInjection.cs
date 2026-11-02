using FastEndpointsTemplate.Application.Common.Interfaces;
using FastEndpointsTemplate.Infrastructure.Persistence;
using FastEndpointsTemplate.Infrastructure.Persistence.Interceptors;
using FastEndpointsTemplate.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FastEndpointsTemplate.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<AuditableEntityInterceptor>();
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? "Data Source=app.db";
        var databaseProvider = configuration["DatabaseProvider"] ?? "Sqlite";

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
            options.AddInterceptors(interceptor);

            if (databaseProvider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                });
            }
            else
            {
                options.UseSqlite(connectionString);
            }
        });

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        // Configuración de Caché Distribuido (Redis con fallback a memoria)
        var redisConnectionString = configuration.GetConnectionString("Redis");
        var healthChecks = services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>(name: "database");

        if (!string.IsNullOrWhiteSpace(redisConnectionString))
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "FastEndpoints:";
            });

            healthChecks.AddRedis(redisConnectionString, name: "redis", tags: ["cache", "ready"]);
        }
        else
        {
            services.AddDistributedMemoryCache();
        }

        services.AddTransient<ICacheService, CacheService>();

        return services;
    }
}
