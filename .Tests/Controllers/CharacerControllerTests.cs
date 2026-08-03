// CharacterControllertests.cs

using Yggdrasil.Tests.Util;

namespace Yggdrasil.Tests.Controllers;

public class CharacterControllerTests : IClassFixture<WebApplicationFactory<Program>>{
    private readonly Faker _faker = new();
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public CharacterControllerTests(WebApplicationFactory<Program> factory){
        _factory = ControllerUtil.Setup(factory);
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_Ok(){
        var response = await _client.GetAsync("/api/character/all");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("1")]
    [InlineData("10")]
    [InlineData("150")]
    public async Task GetAll_OkOnCount(string count){
        var response = await _client.GetAsync($"/api/character/all?count={count}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("-1")]
    public async Task GetAll_BadRequest(string count){
        var response = await _client.GetAsync($"/api/character/all?count={count}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetOne_Ok(){
        var character = ControllerUtil.CreateCharacter(_factory);
        var response = await _client.GetAsync($"/api/character/{character.ID}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetOne_NotFound(){
        var response = await _client.GetAsync($"/api/character/{_faker.Random.Guid()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Theory]
    [InlineData("abc")]
    [InlineData("123")]
    public async Task GetOne_BadRequest(string input){
        var response = await _client.GetAsync($"/api/character/{input}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_Ok(){
        var body = new {
            name = _faker.Name.FullName(),
            description = _faker.Lorem.Lines(),
            personality = _faker.Lorem.Lines(),
            gender = _faker.Person.Gender.ToString()
        };
        var content = new StringContent(
            JsonSerializer.Serialize(body),
            System.Text.Encoding.UTF8,
            "application/json"
        );
        var response = await _client.PostAsync("/api/character/create", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("", "desc", "pers", "gen")]
    [InlineData("name", "", "pers", "gen")]
    [InlineData("name", "desc", "", "gen")]
    [InlineData("name", "desc", "pers", "")]
    public async Task Create_MissingRequiredReturnBadRequest(string name, string description, string personality, string gender){
        var body = new { name, description, personality, gender };
        var content = new StringContent(
            JsonSerializer.Serialize(body),
            System.Text.Encoding.UTF8,
            "application/json"
        );
        var response = await _client.PostAsync("/api/character/create", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Ok(){
        var character = ControllerUtil.CreateCharacter(_factory);
        var response = await _client.DeleteAsync($"/api/character/{character.ID}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    [Fact]
    public async Task Delete_NotFound(){
        var response = await _client.DeleteAsync($"/api/character/{_faker.Random.Guid()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
