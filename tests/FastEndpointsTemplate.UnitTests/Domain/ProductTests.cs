using FastEndpointsTemplate.Domain.Common;
using FastEndpointsTemplate.Domain.Entities;
using FastEndpointsTemplate.Domain.Events;
using FluentAssertions;
using Xunit;

namespace FastEndpointsTemplate.UnitTests.Domain;

public class ProductTests
{
    [Fact]
    public void Create_WithValidParameters_ShouldReturnSuccessAndPublishEvent()
    {
        // Arrange
        var name = "Mechanical Keyboard";
        var sku = "TECH-KEY-001";
        var description = "RGB Mechanical Keyboard";
        var price = 99.99m;
        var stock = 50;

        // Act
        var result = Product.Create(name, sku, description, price, stock);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be(name);
        result.Value.Sku.Should().Be(sku);
        result.Value.Price.Should().Be(price);
        result.Value.StockQuantity.Should().Be(stock);
        result.Value.IsActive.Should().BeTrue();

        result.Value.DomainEvents.Should().ContainSingle(e => e is ProductCreatedDomainEvent);
    }

    [Theory]
    [InlineData("", "SKU123", 10, 5)]
    [InlineData("   ", "SKU123", 10, 5)]
    [InlineData("Name", "", 10, 5)]
    [InlineData("Name", "SKU123", -1, 5)]
    [InlineData("Name", "SKU123", 10, -5)]
    public void Create_WithInvalidParameters_ShouldReturnFailure(
        string name, string sku, decimal price, int stock)
    {
        // Act
        var result = Product.Create(name, sku, "desc", price, stock);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public void UpdateStock_WhenStockDropsBelowZero_ShouldReturnConflictFailure()
    {
        // Arrange
        var product = Product.Create("Mouse", "TECH-MOU-001", "Gaming Mouse", 49.99m, 5).Value;

        // Act
        var result = product.UpdateStock(-10);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Product.InsufficientStock");
        result.Error.Type.Should().Be(ErrorType.Conflict);
    }
}
