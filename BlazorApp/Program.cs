using BlazorApp.Components;
using BlazorApp.Services;

var builder = WebApplication.CreateBuilder(args);

// порт клиентского приложения
builder.WebHost.UseUrls("http://localhost:5010");

// Blazor интерактивный сервер
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// HttpClient для WebAPI
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5000/")
});

// твои сервисы
builder.Services.AddScoped<IUserService, HttpUserService>();
builder.Services.AddScoped<IPostService, HttpPostService>();
builder.Services.AddScoped<ICommentService, HttpCommentService>();

var app = builder.Build();

app.UseStaticFiles();

// для интерактивного рендера в .NET 8 часто нужен антифорджери
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();