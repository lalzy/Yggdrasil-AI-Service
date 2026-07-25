using Microsoft.EntityFrameworkCore;
using AIService.Data;
using AIService.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddRazorPages().AddRazorPagesOptions(options => {
    options.RootDirectory = "/src/Pages";
});



var app = builder.Build();
app.MapEndpoints();

app.MapRazorPages();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/test", () => Results.Ok("works"));
app.Run();