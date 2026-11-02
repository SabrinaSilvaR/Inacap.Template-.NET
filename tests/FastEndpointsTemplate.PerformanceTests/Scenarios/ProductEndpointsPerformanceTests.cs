using System.Net.Http.Json;
using FastEndpoints.Testing;
using FastEndpointsTemplate.Api.Endpoints.Products;
using FastEndpointsTemplate.PerformanceTests.Fixtures;
using FluentAssertions;
using NBomber.Contracts;
using NBomber.CSharp;
using Xunit;

namespace FastEndpointsTemplate.PerformanceTests.Scenarios;

#pragma warning disable xUnit1051

public class ProductEndpointsPerformanceTests(PerformanceAppFixture fixture) : TestBase<PerformanceAppFixture>
{
    [Fact]
    public async Task ListProducts_PerformanceTest_ShouldMeetSla()
    {
        var client = fixture.Client;

        // Crear producto base para poblar catálogo
        var initialProduct = new CreateProductRequest(
            Name: "Perf Catalog Base Item",
            Sku: $"BASE-{Guid.NewGuid():N}"[..18],
            Description: "Initial item for load testing",
            Price: 49.99m,
            StockQuantity: 1000
        );
        var setupResponse = await client.PostAsJsonAsync("/api/products", initialProduct);
        setupResponse.EnsureSuccessStatusCode();

        var scenario = Scenario.Create("list_products_perf_scenario", async context =>
        {
            var response = await client.GetAsync("/api/products");

            return response.IsSuccessStatusCode
                ? Response.Ok()
                : Response.Fail();
        })
        .WithoutWarmUp()
        .WithLoadSimulations(
            Simulation.KeepConstant(copies: 10, during: TimeSpan.FromSeconds(3))
        );

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .WithReportFolder("reports/perf_list_products")
            .Run();

        var scenarioStats = stats.ScenarioStats.Get("list_products_perf_scenario");

        // Verificación de SLAs
        scenarioStats.Fail.Request.Count.Should().Be(0, "todas las solicitudes concurrentes deben responder exitosamente");
        scenarioStats.Ok.Latency.Percent95.Should().BeLessThan(250, "el percentil 95 debe responder dentro del SLA");
    }

    [Fact]
    public async Task GetProductById_PerformanceTest_ShouldMeetSla()
    {
        var client = fixture.Client;

        // Crear un producto para probar consulta individual concurrente
        var product = new CreateProductRequest(
            Name: "Perf Target Item",
            Sku: $"GETID-{Guid.NewGuid():N}"[..18],
            Description: "Item for single get performance benchmark",
            Price: 199.99m,
            StockQuantity: 50
        );
        var setupResponse = await client.PostAsJsonAsync("/api/products", product);
        setupResponse.EnsureSuccessStatusCode();
        var created = await setupResponse.Content.ReadFromJsonAsync<CreateProductResponse>();
        created.Should().NotBeNull();

        var scenario = Scenario.Create("get_product_by_id_perf_scenario", async context =>
        {
            var response = await client.GetAsync($"/api/products/{created!.Id}");

            return response.IsSuccessStatusCode
                ? Response.Ok()
                : Response.Fail();
        })
        .WithoutWarmUp()
        .WithLoadSimulations(
            Simulation.KeepConstant(copies: 10, during: TimeSpan.FromSeconds(3))
        );

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .WithReportFolder("reports/perf_get_by_id")
            .Run();

        var scenarioStats = stats.ScenarioStats.Get("get_product_by_id_perf_scenario");

        // Verificación de SLAs
        scenarioStats.Fail.Request.Count.Should().Be(0, "las consultas por ID no deben arrojar errores");
        scenarioStats.Ok.Latency.Percent95.Should().BeLessThan(200, "el percentil 95 de lectura puntual debe ser ultrarrápido");
    }

    [Fact]
    public async Task CreateProduct_PerformanceTest_ShouldMeetSla()
    {
        var client = fixture.Client;
        var counter = 0;

        var scenario = Scenario.Create("create_product_perf_scenario", async context =>
        {
            var id = Interlocked.Increment(ref counter);
            var request = new CreateProductRequest(
                Name: $"Concurrent Product {id}",
                Sku: $"CONC-{id:D5}-{Guid.NewGuid():N}"[..22],
                Description: "Concurrent creation stress test",
                Price: 10.0m + (id % 50),
                StockQuantity: 100
            );

            var response = await client.PostAsJsonAsync("/api/products", request);

            return response.IsSuccessStatusCode
                ? Response.Ok()
                : Response.Fail();
        })
        .WithoutWarmUp()
        .WithLoadSimulations(
            Simulation.KeepConstant(copies: 5, during: TimeSpan.FromSeconds(3))
        );

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .WithReportFolder("reports/perf_create_product")
            .Run();

        var scenarioStats = stats.ScenarioStats.Get("create_product_perf_scenario");

        // Verificación de SLAs
        scenarioStats.Fail.Request.Count.Should().Be(0, "la creación concurrente no debe arrojar errores 5xx");
        scenarioStats.Ok.Latency.Percent95.Should().BeLessThan(350, "la persistencia y validación concurrente debe cumplir el SLA");
    }
}
