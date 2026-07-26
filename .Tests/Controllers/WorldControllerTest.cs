// WorldControllerTests.cs

using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Net;
using Bogus;

public class WorldControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Faker _faker = new();
    private readonly HttpClient _client;

    public WorldControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/world/all");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("-1")]
    public async Task GetAll_BadCount_ReturnsBadRequest(string count)
    {
        var response = await _client.GetAsync($"/api/world/all?count={count}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_NullName_ReturnsBadRequest()
    {
        var body = new { description = _faker.Lorem.Lines() };
        var content = new StringContent(
            JsonSerializer.Serialize(body),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("/api/world/create", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_NullDescription_ReturnsBadRequest()
    {
        var body = new { name = _faker.Name.FullName() };
        var content = new StringContent(
            JsonSerializer.Serialize(body),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("/api/world/create", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}