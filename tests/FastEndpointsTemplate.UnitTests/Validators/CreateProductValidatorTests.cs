using FastEndpointsTemplate.Api.Endpoints.Products;
using FluentAssertions;
using Xunit;

namespace FastEndpointsTemplate.UnitTests.Validators;

public class CreateProductValidatorTests
{
    private readonly CreateProductValidator _validator = new();

    [Fact]
    public void Validate_WhenRequestIsValid_ShouldNotHaveAnyErrors()
    {
        // Arrange
        var request = new CreateProductRequest("Monitor 4K", "TECH-MON-001", "27 inch IPS", 399.99m, 15);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WhenNameAndSkuAreEmpty_ShouldHaveValidationErrors()
    {
        // Arrange
        var request = new CreateProductRequest("", "", "desc", -10, -5);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Name));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Sku));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Price));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(request.StockQuantity));
    }
}
