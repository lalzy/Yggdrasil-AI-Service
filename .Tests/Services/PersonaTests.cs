// PersonaTests

using Yggdrasil.Tests.Factories;
using Yggdrasil.Services;
using Yggdrasil.Models;
using Yggdrasil.DTO;
using Yggdrasil.Util;

namespace Yggdrasil.Tests.Services;

public class PersonaTests : DatabaseTestBase{
    private readonly PersonaService _service;
    private readonly Faker _faker = new();

    public PersonaTests(DatabaseFixture fixture) :base(fixture){
        _service = new PersonaService(fixture.CreateContext());
    }

    // Helpers
    private void CreatePersonas(int count = 1){
        var context = _fixture.CreateContext();
        for (int i = 0; i < count; i++){
            PersonaFactory.Create(context);
        }
    }

    // Tests
    [Theory]
    [InlineData(5)]
    [InlineData(30)]
    [InlineData(150)]
    public void GetAll_GetAllMade(int count){
        CreatePersonas(count);

        var fetch = _service.GetAll().Data!;

        Assert.Equal(count, fetch.Count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(30)]
    public void GetAll_GetOnlyRequestedAmount(int count){
        CreatePersonas(100);

        var fetch = _service.GetAll(count).Data!;
        Assert.Equal(count, fetch.Count);
    }

    [Fact]
    public void GetAll_ReturnsCorrectServiceResultType(){
        CreatePersonas(3);
        var fetch = _service.GetAll();

        Assert.IsType<ServiceResult<List<PersonaSummary>>>(fetch);
    }

    [Fact]
    public void GetAll_EmptyReturnsEmtpy(){
        var fetch = _service.GetAll().Data!;

        Assert.Empty(fetch);
    }

    [Fact]
    public void GetAll_LessThanOneCountThrows(){
        CreatePersonas(3);
        Assert.Throws<ArgumentException>(() => _service.GetAll(-1));
    }
}
