// CharacterServiceTests.cs
using Bogus;
using AutoBogus;
using Yggdrasil.Tests.Factories;
using Yggdrasil.Services;
using Yggdrasil.Models;
using Yggdrasil.DTO;
using Yggdrasil.Util;
using yggdrasil.Util;
using Xunit.Sdk;

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
            CharacterFactory.Create(context, world_ID);
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
    public void GetAll_GetOnlyRequestedAmount()
    {
        int count = 20;
        int toGet = 5;
        CreateCharacters(count);
        var fetch = _service.GetAll(toGet).Data!;

        Assert.Equal(toGet, fetch.Count);
    }

    [Fact]
    public void GetAll_ReturnsCorrectServiceResultType()
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
        var character = CharacterFactory.Create(_fixture.CreateContext());
        var fetch = _service.GetOne(character.ID).Data!;

        Assert.Equivalent(character, fetch);
    }

    [Fact]
    public void GetOne_GetCorrectFromMany()
    {
        var context = _fixture.CreateContext();
        var characterNotToGet = CharacterFactory.Create(context);
        var characterToGet = CharacterFactory.Create(context);

        var fetch = _service.GetOne(characterToGet.ID).Data!;

        Assert.Equivalent(characterToGet, fetch);
        Assert.Throws<EquivalentException>(()=> Assert.Equivalent(characterNotToGet, fetch));
    }

    [Fact]
    public void GetOne_InvalidGuidThrows()
    {
        CreateCharacters(2);
        Assert.Throws<KeyNotFoundException>(()=>_service.GetOne(_faker.Random.Uuid()));
    }

    [Fact]
    public void Create_Success()
    {
        var request = AutoFaker.Generate<CharacterRequest>();
        var convertedRequest = request.ConvertModelToDTO<Character>();
        var character = _service.Create(request).Data!;
        var ID = character.ID;
        character.ID = Guid.Empty; // Request doesn't have ID.
        Assert.Equivalent(convertedRequest, character, strict:false);

        // Check for DB:
        var fetched = _fixture.CreateContext().Set<Character>().FirstOrDefault(c=>c.ID == ID);

        Assert.Throws<EquivalentException>(()=>Assert.Equivalent(character, fetched));
    }
}