// WorldControllerTests.cs

using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Net;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Yggdrasil.Data;
using Microsoft.Data.Sqlite;
using Yggdrasil.Models;
using Yggdrasil.Tests.Factories;
using Yggdrasil.Tests.Util;

namespace Yggdrasil.Tests.Controllers;

public class WorldControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Faker _faker = new();
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public WorldControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = ControllerUtil.Setup(factory);
        _client = _factory.CreateClient();
    }

    // Tests

    [Fact]
    public async Task GetAll_Ok()
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
    public async Task GetWorld_Ok()
    {
        var world = ControllerUtil.CreateWorld(_factory);
        var response = await _client.GetAsync($"/api/world/{world.ID}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetWorld_NotFound()
    {
        var response = await _client.GetAsync($"/api/world/{_faker.Random.Guid()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateWorld_Ok()
    {
        var body = new { name = _faker.Name.FullName(), description= _faker.Lorem.Lines() };
        var content = new StringContent(
            JsonSerializer.Serialize(body),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("/api/world/create", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateWorld_NullName_ReturnsBadRequest()
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
    public async Task CreateWorld_NullDescription_ReturnsBadRequest()
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

    [Fact]
    public async Task DeleteWorld_Ok()
    {
        var world = ControllerUtil.CreateWorld(_factory);
        var response = await _client.DeleteAsync($"/api/world/{world.ID}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteWorld_NotFound()
    {
        var response = await _client.DeleteAsync($"/api/world{_faker.Random.Guid()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

[Fact]
public async Task AddCharacter_Ok()
{
    var world = ControllerUtil.CreateWorld(_factory);
    var character = ControllerUtil.CreateCharacter(_factory);
    
    var response = await _client.PostAsync($"/api/world/{world.ID}/characters/{character.ID}", null);
    
    var body = await response.Content.ReadAsStringAsync();
    Console.WriteLine("======= START =====");
    Console.WriteLine(body);
    Console.WriteLine("======END!");
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
}

    [Fact]
    public async Task AddCharacter_WorldNotFound()
    {
        var character = ControllerUtil.CreateCharacter(_factory);
        var response = await _client.PostAsync($"/api/world/{_faker.Random.Guid()}/characters/{character.ID}", null);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AddCharacter_CharacterNotFound()
    {
        var world = ControllerUtil.CreateWorld(_factory);
        var response = await _client.PostAsync($"/api/world/{world.ID}/characters/{_faker.Random.Guid()}", null);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task RemoveCharacter_Ok()
    {
        var world = ControllerUtil.CreateWorld(_factory);
        var character = ControllerUtil.CreateCharacter(_factory, world.ID);
        
        var response = await _client.DeleteAsync($"/api/world/{world.ID}/characters/{character.ID}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task RemoveCharacter_WorldNotFound()
    {
        var character = ControllerUtil.CreateCharacter(_factory);
        var response = await _client.DeleteAsync($"/api/world/{_faker.Random.Guid()}/characters/{character.ID}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task RemoveCharacter_CharacterNotFound()
    {
        var world = ControllerUtil.CreateWorld(_factory);
        var response = await _client.DeleteAsync($"/api/world/{world.ID}/characters/{_faker.Random.Guid()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
