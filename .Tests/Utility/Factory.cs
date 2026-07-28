using Bogus;
using Microsoft.EntityFrameworkCore;
using Yggdrasil.Data;
using Yggdrasil.Models;
namespace Yggdrasil.Tests.Utility;

public static class Factory
{

    public static World CreateWorld(AppDbContext db, string? instruction = null)
    {
        Faker faker = new();
        var world = new World
        {
            Name = faker.Lorem.Lines(),
            Description = faker.Lorem.Lines(),
            NarratorInstruction = instruction,
        };
        db.Set<World>().Add(world);
        db.SaveChanges();
        return world;
    }

    public static World CreateWorld(DatabaseFixture fixture, string? instruction = null)
    {
        using var db = fixture.CreateContext();
        return CreateWorld(db, instruction);
    }
}