// Program.cs
using EvaluadorInteligente.Data;          // <- tu namespace Data
using Microsoft.EntityFrameworkCore;      // <- EF Core

var builder = WebApplication.CreateBuilder(args);

// 1️⃣  Conexión a PostgreSQL (usa la cadena "Default" de appsettings.Development.json)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// 2️⃣  MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 3️⃣  Middleware y rutas
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
