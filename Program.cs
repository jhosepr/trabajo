using Microsoft.EntityFrameworkCore;
using practica.Data;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURACIÓN DE SQLITE ---

// 1. Configuración para practicaContext
builder.Services.AddDbContext<practicaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("practicaContext")
    ?? throw new InvalidOperationException("Connection string 'practicaContext' not found.")));

// 2. Obtener la cadena de conexión para ApplicationDbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 3. Registrar ApplicationDbContext usando SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// -------------------------------

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();