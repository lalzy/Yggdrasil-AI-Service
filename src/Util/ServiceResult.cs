namespace Yggdrasil.Util;

public record ServiceResult<T>(T? Data, string? Error = null)
{
    public bool Success => Error == null;
}