// CharacterFactory.cs

using AutoBogus;
using Yggdrasil.Models;
using Yggdrasil.Data;

namespace Yggdrasil.Tests.Factories;

public class WorldFactory{
    public static World Create(AppDbContext db){
        var world = AutoFaker.Generate<World>();
        world.Characters.Clear();
        
        db.Set<World>().Add(world);
        db.SaveChanges();
        return world;
    }

    public static World Create(DatabaseFixture fixture, Guid? world_ID = null){
        using var db = fixture.CreateContext();
        return Create(db);
    }
}
