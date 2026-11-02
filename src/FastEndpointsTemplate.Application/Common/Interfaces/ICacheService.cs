namespace FastEndpointsTemplate.Application.Common.Interfaces;

/// <summary>
/// Abstracción agnóstica para operaciones de almacenamiento en caché distribuido (Redis / Memory).
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Obtiene un valor tipado desde la caché por su clave.
    /// </summary>
    Task<T?> GetAsync<T>(string key, CancellationToken ct = default);

    /// <summary>
    /// Almacena un valor en la caché con un tiempo de expiración opcional.
    /// </summary>
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken ct = default);

    /// <summary>
    /// Elimina una clave de la caché.
    /// </summary>
    Task RemoveAsync(string key, CancellationToken ct = default);
}
