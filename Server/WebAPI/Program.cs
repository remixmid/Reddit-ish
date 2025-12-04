using EfcRepositories;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using AppDbContext = EfcRepositories.AppContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var dbPath = Path.Combine(System.AppContext.BaseDirectory, "app.db");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite($"Data Source={dbPath}");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepository, EfcUserRepository>();
builder.Services.AddScoped<IPostRepository, EfcPostRepository>();
builder.Services.AddScoped<ICommentRepository, EfcCommentRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();