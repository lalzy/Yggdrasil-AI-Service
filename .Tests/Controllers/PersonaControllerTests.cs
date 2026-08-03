// PersonaControllerTests.cs

using Yggdrasil.Tests.Util;

namespace Yggdrasil.Tests.Controllers;

public class PersonaControllerTests : IClassFixture<WebApplicationFactory<Program>>{
    private readonly Faker _faker = new();
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public PersonaControllerTests(WebApplicationFactory<Program> factory){
        _factory = ControllerUtil.Setup(factory);
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_Ok(){
        var response = await _client.GetAsync("/api/persona/all");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("1")]
    [InlineData("10")]
    [InlineData("150")]
    public async Task GetAll_OkOnCount(string count){
        var response = await _client.GetAsync($"/api/Persona/all?count={count}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("-1")]
    public async Task GetAll_BadRequest(string count){
        var response = await _client.GetAsync($"/api/Persona/all?count={count}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetOne_Ok(){
        var persona = ControllerUtil.CreatePersona(_factory);
        var response = await _client.GetAsync($"/api/persona/{persona.ID}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetOne_NotFound(){
        var response = await _client.GetAsync($"/api/persona/{_faker.Random.Guid()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("123")]
    public async Task GetOne_BadRequest(string input){
        var response = await _client.GetAsync($"/api/persona/{input}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_OK(){
        var body = new
        {
            name = _faker.Name.FullName(),
            description = _faker.Lorem.Lines(),
            gender = _faker.Person.Gender.ToString()
        };

        var content = new StringContent(
            JsonSerializer.Serialize(body),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("/api/persona/create", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
