using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Farmacia.Models;
using Farmacia.Data;
using practica.Models;

namespace practica.Data
{
    public static class SeedData
    {
        public static async Task InicializarRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();

            string[] roles = { "Administrador", "Farmaceutico", "Cliente" };

            foreach (var rol in roles)
            {
                if (!await roleManager.RoleExistsAsync(rol))
                    await roleManager.CreateAsync(new IdentityRole(rol));
            }

            var adminEmail = "admin@farmacia.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new Usuario
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Nombre = "Admin",
                    Apellido = "Sistema",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Administrador");
            }
        }

        public static async Task InicializarDatos(IServiceProvider serviceProvider)
        {
            using var context = new practicaContext(
                serviceProvider.GetRequiredService<DbContextOptions<practicaContext>>());

            // Si ya hay categorías, asumimos que ya se pobló la base de datos
            if (context.Categorias.Any()) return; 

            // 1. Poblar Categorías
            var categs = new Categoria[]
            {
                new Categoria { Nombre = "Analgésicos", Descripcion = "Alivio del dolor y fiebre" },
                new Categoria { Nombre = "Antibióticos", Descripcion = "Combate infecciones bacterianas" },
                new Categoria { Nombre = "Vitaminas", Descripcion = "Suplementos y multivitamínicos" },
                new Categoria { Nombre = "Ansiolíticos", Descripcion = "Control de ansiedad bajo receta" }
            };
            context.Categorias.AddRange(categs);
            await context.SaveChangesAsync();

            // 2. Poblar Estantes
            var estantes = new Estante[]
            {
                new Estante { Nombre = "A1", Ubicacion = "Pasillo Principal", Descripcion = "Productos de alta rotación" },
                new Estante { Nombre = "B2-Frio", Ubicacion = "Refrigerador 1", Descripcion = "Requiere cadena de frío" },
                new Estante { Nombre = "C3", Ubicacion = "Almacén Derecho", Descripcion = "Stock de reserva" }
            };
            context.Estantes.AddRange(estantes);
            await context.SaveChangesAsync();

            // 3. Poblar Medicamentos con diferentes estados
            var meds = new List<Medicamento>
            {
                new Medicamento { 
                    Nombre = "Paracetamol 500mg", Precio = 0.50m, Stock = 100, 
                    FechaVencimiento = DateTime.Today.AddMonths(18), 
                    CategoriaId = categs[0].Id, EstanteId = estantes[0].Id, Estado = true 
                },
                new Medicamento { 
                    Nombre = "Amoxicilina Suspensión", Precio = 11.20m, Stock = 15, 
                    FechaVencimiento = DateTime.Today.AddDays(-15), 
                    CategoriaId = categs[1].Id, EstanteId = estantes[1].Id, Estado = true 
                },
                new Medicamento { 
                    Nombre = "Neurobion Inyectable", Precio = 25.00m, Stock = 30, 
                    FechaVencimiento = DateTime.Today.AddDays(20), 
                    CategoriaId = categs[2].Id, EstanteId = estantes[0].Id, Estado = true 
                },
                new Medicamento { 
                    Nombre = "Clonazepam 2mg", Precio = 15.50m, Stock = 4, 
                    FechaVencimiento = DateTime.Today.AddMonths(24), 
                    CategoriaId = categs[3].Id, EstanteId = estantes[1].Id, Estado = true 
                },
                new Medicamento { 
                    Nombre = "Vitamina C Efervescente", Precio = 7.80m, Stock = 80, 
                    FechaVencimiento = DateTime.Today.AddDays(55), 
                    CategoriaId = categs[2].Id, EstanteId = estantes[2].Id, Estado = true 
                }
            };
            context.Medicamentos.AddRange(meds);
            await context.SaveChangesAsync();
        }
    }
}