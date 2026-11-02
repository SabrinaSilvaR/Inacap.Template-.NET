using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FastEndpointsTemplate.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
