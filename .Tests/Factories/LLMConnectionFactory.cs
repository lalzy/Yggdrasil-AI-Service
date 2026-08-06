// LLMConnectionFactory.cs

using Yggdrasil.Models;
using Yggdrasil.Data;

public class LLMConnectionFactory{
    public static LLMConnection Create (AppDbContext db, SupportedProviders? provider=null){
        var faker = new Faker();
        var connection = AutoFaker.Generate<LLMConnection>();
        connection.APIKey = faker.Random.AlphaNumeric(32);
        connection.URL = faker.Internet.Url();
        if (provider != null) connection.Provider = provider.Value;

        db.Set<LLMConnection>().Add(connection);
        db.SaveChanges();
        return connection;
    }

    public static LLMConnection Create(DatabaseFixture fixture, SupportedProviders? provider=null){
        using var db = fixture.CreateContext();
        return Create(db, provider);
    }
}
