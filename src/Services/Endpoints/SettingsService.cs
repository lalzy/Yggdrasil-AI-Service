using Yggdrasil.Models;
using Yggdrasil.Util;

namespace Yggdrasil.Services;

public class SettingsService{
    private readonly AppDbContext _db;

    public SettingsService(AppDbContext db)
    {
        _db = db;
    }

    public ServiceResult<Themes> getTheme()
    {
        var theme = _db.Set<Settings>().Select(s=>s.Theme).Distinct().FirstOrDefault();
        return new ServiceResult<Themes>(theme);
    }
}