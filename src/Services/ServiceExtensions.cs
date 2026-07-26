// ServiceExtensions.cs
using System.Reflection;
using Microsoft.EntityFrameworkCore;

public static class ServiceExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        var serviceTypes = Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.IsClass && !t.IsAbstract 
            && t.Namespace == "Yggdrasil.Services"
            && !t.IsSubclassOf(typeof(DbContext))
            && !t.IsNested);

        foreach (var type in serviceTypes)
        {
            services.AddScoped(type);
        }
    }
}