using FastEndpointsTemplate.Application.Common.Interfaces;

namespace FastEndpointsTemplate.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
