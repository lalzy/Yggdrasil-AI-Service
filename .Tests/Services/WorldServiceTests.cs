// WorldServiceTests.cs

using Yggdrasil.Models;
using Yggdrasil.Services;
using Yggdrasil.Data;
using Bogus;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Yggdrasil.DTO;

public class WorldServiceTests : IClassFixture<DatabaseFixture>
{
    private readonly WorldService _service;
    private readonly Faker _faker = new();

    public WorldServiceTests(DatabaseFixture fixture)
    {
        _service = new WorldService(fixture.Db);
    }
    // Helpers
    private WorldRequest CreateWorldRequest(string? NarratorInstruction = null)
    {
        return new WorldRequest
        {
            Name = _faker.Name.FullName(),
            Description = _faker.Lorem.Lines(),
            NarratorInstruction = NarratorInstruction,
        };
    }

    private void createWorlds (int count=1)
    {
        for(int i = 0; i < count; i++)
        {
            _service.createWorld(CreateWorldRequest());
        }
    }

    [Fact]
    public void getWorlds_GetAllWorldsMade()
    {
        int count = _faker.Random.Int(3,10);
        createWorlds(count);
        var fetched = _service.getWorlds().Data;
        Assert.Equal(count, fetched.Count);
    }

    [Fact]
    public void getWorlds_GetOnlyRequestedAmount()
    {
        int count = 10;
        int toGet = _faker.Random.Int(1,7);
        createWorlds(count);

        var fetched = _service.getWorlds(toGet).Data;
        Assert.Equal(toGet, fetched.Count);
    }
}