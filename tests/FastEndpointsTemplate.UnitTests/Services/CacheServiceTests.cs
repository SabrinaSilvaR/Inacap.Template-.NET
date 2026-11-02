using System.Text.Json;
using FastEndpointsTemplate.Infrastructure.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using NSubstitute;
using Xunit;

namespace FastEndpointsTemplate.UnitTests.Services;

public class CacheServiceTests
{
    private readonly IDistributedCache _cacheMock = Substitute.For<IDistributedCache>();
    private readonly CacheService _sut;

    public CacheServiceTests()
    {
        _sut = new CacheService(_cacheMock);
    }

    [Fact]
    public async Task GetAsync_WhenKeyExists_ShouldReturnDeserializedObject()
    {
        // Arrange
        const string key = "test-key";
        var expectedData = new TestPayload(1, "Sample Data");
        var serializedBytes = JsonSerializer.SerializeToUtf8Bytes(expectedData);

        _cacheMock.GetAsync(key, Arg.Any<CancellationToken>())
            .Returns(serializedBytes);

        // Act
        var result = await _sut.GetAsync<TestPayload>(key);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(expectedData.Id);
        result.Name.Should().Be(expectedData.Name);
    }

    [Fact]
    public async Task GetAsync_WhenKeyDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        const string key = "missing-key";
        _cacheMock.GetAsync(key, Arg.Any<CancellationToken>())
            .Returns((byte[]?)null);

        // Act
        var result = await _sut.GetAsync<TestPayload>(key);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task SetAsync_WhenValueProvided_ShouldCallDistributedCacheSetAsync()
    {
        // Arrange
        const string key = "store-key";
        var payload = new TestPayload(42, "Cached Item");

        // Act
        await _sut.SetAsync(key, payload, TimeSpan.FromMinutes(10));

        // Assert
        await _cacheMock.Received(1).SetAsync(
            key,
            Arg.Any<byte[]>(),
            Arg.Is<DistributedCacheEntryOptions>(o => o.AbsoluteExpirationRelativeToNow == TimeSpan.FromMinutes(10)),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RemoveAsync_ShouldCallDistributedCacheRemoveAsync()
    {
        // Arrange
        const string key = "delete-key";

        // Act
        await _sut.RemoveAsync(key);

        // Assert
        await _cacheMock.Received(1).RemoveAsync(key, Arg.Any<CancellationToken>());
    }

    private record TestPayload(int Id, string Name);
}
