// PersonaFactory.cs

using Yggdrasil.Models;
using Yggdrasil.Data;

namespace Yggdrasil.Tests.Factories;

public class PersonaFactory{
    public static Persona Create(AppDbContext db){
        var persona = AutoFaker.Generate<Persona>();
        db.Set<Persona>().Add(persona);
        db.SaveChanges();
        return persona;
    }

    public static Persona Create(DatabaseFixture fixture){
        using var db = fixture.CreateContext();
        return Create(db);
    }
}
