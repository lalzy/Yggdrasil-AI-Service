// WorldServiceTests.cs

using Yggdrasil.Services;
using Bogus;
using Yggdrasil.DTO;
using Yggdrasil.Models;
using Microsoft.EntityFrameworkCore;

public class WorldServiceTests : DatabaseTestBase
{
    private readonly WorldService _service;
    private readonly Faker _faker = new();

    public WorldServiceTests(DatabaseFixture fixture) : base(fixture)
    {
        _service = new WorldService(fixture.CreateContext());
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

    [Fact]
    public void createWorlds_Success()
    {
        var request = CreateWorldRequest("someCustomInstruction");
        var result = _service.createWorld(request).Data;

        Assert.Equivalent(request, result);
    }

    [Fact]
    public void createWorlds_DefaultInstruction()
    {
        var result = _service.createWorld(CreateWorldRequest()).Data;

        Assert.Equal(Yggdrasil.Models.Settings.NARRATION_PROMPT, result.NarratorInstruction);
    }

    [Fact]
    public void DeleteWorlds_DeletesTheWorld()
    {
        var world_ID = _service.createWorld(CreateWorldRequest()).Data!.ID;

        Assert.True(_service.getWorld(world_ID).Data != null);

        _service.DeleteWorld(world_ID);

        Assert.True(_service.getWorld(world_ID).Data == null);
    }

    [Fact]
    public void DeleteWorlds_OnlyDeleteSelectedWorld()
    {
        createWorlds(1);
        var world_ID = _service.createWorld(CreateWorldRequest()).Data!.ID;
        createWorlds(1);
        Assert.Equal(3, _service.getWorlds().Data!.Count);
        _service.DeleteWorld(world_ID);
        var fetch = _service.getWorlds().Data!;
        Assert.Equal(2, fetch.Count);
        Assert.DoesNotContain(fetch, w=>w.world_ID == world_ID);
    }
}