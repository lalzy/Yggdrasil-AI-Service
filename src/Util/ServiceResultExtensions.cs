// ServiceResultExtensions.cs
using Microsoft.AspNetCore.Mvc;

namespace Yggdrasil.Util;

public static class ServiceResultExtensions
{
    public static IActionResult ToResponse<T>(this ServiceResult<T> result)
    {
        if (result.Success)
            return new ObjectResult(result.Data) { StatusCode = result.StatusCode };

        return new ObjectResult(new { error = result.Error }) { StatusCode = result.StatusCode };
    }

    public static IActionResult SafeExecute<T>(Func<ServiceResult<T>> action)
    {
        try
        {
            return action().ToResponse();
        }
        catch (Exception ex)
        {
            return ServiceResult<T>.InternalError(ex.Message).ToResponse();
        }
    }
}