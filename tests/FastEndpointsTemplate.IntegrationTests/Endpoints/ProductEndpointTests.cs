using System.Net;
using System.Net.Http.Json;
using FastEndpoints;
using FastEndpoints.Testing;
using FastEndpointsTemplate.Api.Endpoints.Products;
using FastEndpointsTemplate.Application.Features.Products.Dtos;
using FastEndpointsTemplate.IntegrationTests.Fixtures;
using FluentAssertions;
using Xunit;

namespace FastEndpointsTemplate.IntegrationTests.Endpoints;

#pragma warning disable xUnit1051

public class ProductEndpointTests(AppFixture fixture) : TestBase<AppFixture>
{
    [Fact]
    public async Task CreateProduct_WithValidData_ShouldReturn201Created()
    {
        // Arrange
        var request = new CreateProductRequest(
            Name: "Integration Test Product",
            Sku: $"TEST-SKU-{Guid.NewGuid():N}"[..18],
            Description: "Created from integration test suite",
            Price: 129.99m,
            StockQuantity: 10
        );

        // Act
        var (response, result) = await fixture.Client.POSTAsync<CreateProductEndpoint, CreateProductRequest, CreateProductResponse>(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Name.Should().Be(request.Name);
        result.Price.Should().Be(request.Price);
    }

    [Fact]
    public async Task CreateProduct_WithEmptyName_ShouldReturn400BadRequest()
    {
        // Arrange
        var request = new CreateProductRequest(
            Name: "",
            Sku: "INVALID-SKU",
            Description: "Invalid product",
            Price: 50m,
            StockQuantity: 1
        );

        var ct = TestContext.Current.CancellationToken;
        // Act
        var response = await fixture.Client.PostAsJsonAsync("/api/products", request, ct);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ListProducts_ShouldReturn200OkWithPagedResults()
    {
        var ct = TestContext.Current.CancellationToken;
        // Act
        var response = await fixture.Client.GetAsync("/api/products?page=1&pageSize=10", ct);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<ProductDto>>(ct);
        result.Should().NotBeNull();
        result!.Items.Should().NotBeNull();
    }
}
