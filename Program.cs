using Farmacia.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using practica.Data;
using practica.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<practicaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("practicaContext")
    ?? throw new InvalidOperationException("Connection string 'practicaContext' not found.")));

builder.Services.AddIdentity<Usuario, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<practicaContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccesoDenegado";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await practica.Data.SeedData.InicializarRoles(scope.ServiceProvider);
    await practica.Data.SeedData.InicializarDatos(scope.ServiceProvider);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=PublicPortal}/{id?}");

app.Run();