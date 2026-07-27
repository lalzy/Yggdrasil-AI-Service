using Yggdrasil.Data;
using Yggdrasil.DTO;
using Yggdrasil.Util;
using Microsoft.EntityFrameworkCore;
using Yggdrasil.Models;

namespace Yggdrasil.Services;

public class CharacterService(AppDbContext db)
{
    private readonly AppDbContext _db = db;

    public ServiceResult<List<CharacterSummary>> GetAll(int? count=null)
    {
        var query = _db.Set<Character>().Where<Character>(c=>c.Name != null && c.Description != null).Select(c=> new CharacterSummary(c.ID, c.Name, c.Description));
        if (count.HasValue)
        {
            if(count < 1) throw new ArgumentException(ErrorMessages.LESSTHANONE);
            query = query.Take(count.Value);
        }

        return new (query.ToList());
    }
}