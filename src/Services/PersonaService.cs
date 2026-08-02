// PersonaService.cs

using Yggdrasil.Data;
using Yggdrasil.DTO;
using Yggdrasil.Util;
using Yggdrasil.Models;

namespace Yggdrasil.Services;

public class PersonaService(AppDbContext db){
    private readonly AppDbContext _db = db;

    ///<summary>Get a summary list of all personas</summary>
    ///<param name="count">How many to fetch</param>
    ///<returns>Service result with a list of acharacter summary objects (name, description)</returns>
    public ServiceResult<List<PersonaSummary>> GetAll(int? count=null){
        var query = _db.Set<Persona>().Where<Persona>(p => p.Name != null && p.Description != null && p.Gender != null).Select(p => new PersonaSummary(p.ID, p.Name, p.Description, p.Gender));
        if(count.HasValue){
            if(count.Value < 1) throw new ArgumentException(ErrorMessages.LESSTHANONE);
            query = query.Take(count.Value);
        }
        return new(query.ToList());
    }
}
