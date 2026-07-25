using Microsoft.EntityFrameworkCore;
using Yggdrasil.Services;
using Yggdrasil.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<LLMService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "Yggdrasil - AI Scenarios",
        Version = "v1",
    });
});

builder.Services.AddServices();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db",
    x=>x.MigrationsAssembly("Yggdrasil")));

builder.Services.AddRazorPages().AddRazorPagesOptions(options => {
    options.RootDirectory = "/src/Pages";
});



var app = builder.Build();
app.MapEndpoints();

app.MapRazorPages();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();