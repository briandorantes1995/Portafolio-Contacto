using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Portafolio.Data;
using Portafolio.Services.Mails;
using Portafolio.Services.Auth;
using Portafolio.Services.Storage;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["DB_URL"]
    ?? throw new InvalidOperationException("La variable de entorno DB_URL no está definida.");

builder.Services.AddDbContext<UserDb>(options => options.UseNpgsql(connectionString));
builder.Services.AddRazorPages();
builder.Services.AddScoped<IEmailService, AzureEmailService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/Denied";
        options.Cookie.Name = "PortafolioAuth";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();
app.Run();
