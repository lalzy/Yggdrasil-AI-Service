// CharacterServiceTests.cs
using Bogus;
using Yggdrasil.Tests.Utility;
using Yggdrasil.Services;
using Yggdrasil.Models;
using Yggdrasil.DTO;
using Yggdrasil.Util;

namespace Yggdrasil.Tests.Services;

public class CharacterServiceTests :DatabaseTestBase
{
    private readonly CharacterService _service;
    private readonly Faker _faker = new ();

    public CharacterServiceTests(DatabaseFixture fixture) : base(fixture)
    {
        _service = new CharacterService(fixture.CreateContext());
    }

    // Helpers
    private void CreateCharacters(int count = 1, Guid? world_ID = null)
    {
        var context = _fixture.CreateContext();
        for(int i = 0; i < count; i++)
        {
            Factory.CreateCharacter(context, world_ID);
        }
    }

    [Fact]
    public void GetAll_GetAllMade()
    {
        int count = _faker.Random.Int(20, 30);
        CreateCharacters(count);

        var fetch = _service.GetAll().Data!;

        Assert.Equal(count, fetch.Count);
    }

    [Fact]
    public void GetAll_GetOnlyRequestedAmoun()
    {
        int count = 20;
        int toGet = 5;
        CreateCharacters(count);
        var fetch = _service.GetAll(toGet).Data!;

        Assert.Equal(toGet, fetch.Count);
    }

    [Fact]
    public void GetAll_ReturnsCorrectServiceREsultType()
    {
        CreateCharacters(3);
        var fetch = _service.GetAll();

        Assert.IsType<ServiceResult<List<CharacterSummary>>>(fetch);
    }

    [Fact]
    public void GetAll_ThrowsOnInvalidCount()
    {
        CreateCharacters(3);
        Assert.Throws<ArgumentException>(()=>_service.GetAll(-1));
    }

    [Fact]
    public void GetOne_GetRequested()
    {
        var character = Factory.CreateCharacter(_fixture.CreateContext());
        var fetch = _service.GetOne(character.ID).Data!;

        Assert.Equivalent(character, fetch);
    }

    [Fact]
    public void GetOne_GetCorrectFromMany()
    {
        CreateCharacters(2);
        var character = Factory.CreateCharacter(_fixture.CreateContext());
        CreateCharacters(2);

        var fetch = _service.GetOne(character.ID).Data!;

        Assert.Equivalent(character, fetch);
    }

    [Fact]
    public void GetOne_InvalidGuidThrows()
    {
        CreateCharacters(2);
        Assert.Throws<KeyNotFoundException>(()=>_service.GetOne(_faker.Random.Uuid()));
    }
}