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

            // 2. Crear Farmacéutico (NUEVO BLOQUE)
            var farmaciaEmail = "farmaceutico@farmacia.com";
            if (await userManager.FindByEmailAsync(farmaciaEmail) == null)
            {
                var farmaceutico = new Usuario
                {
                    UserName = farmaciaEmail,
                    Email = farmaciaEmail,
                    Nombre = "Juan",
                    Apellido = "Pérez",
                    EmailConfirmed = true
                };

                // Creamos el usuario con una contraseña temporal
                var result = await userManager.CreateAsync(farmaceutico, "Farma123!");

                if (result.Succeeded)
                {
                    // Asignamos el rol "Farmaceutico" que definiste arriba
                    await userManager.AddToRoleAsync(farmaceutico, "Farmaceutico");
                }
            }

        }
        
        public static async Task InicializarDatos(IServiceProvider serviceProvider)
        {
            using var context = new practicaContext(
                serviceProvider.GetRequiredService<DbContextOptions<practicaContext>>());

            // Si ya hay categorías, asumimos que ya se pobló la base de datos
            if (context.Categorias.Any()) return; 

            // 1. Poblar Categorías (4 originales + 10 nuevas = 14)
            var categs = new Categoria[]
            {
                new Categoria { Nombre = "Analgésicos", Descripcion = "Alivio del dolor y fiebre" },
                new Categoria { Nombre = "Antibióticos", Descripcion = "Combate infecciones bacterianas" },
                new Categoria { Nombre = "Vitaminas", Descripcion = "Suplementos y multivitamínicos" },
                new Categoria { Nombre = "Ansiolíticos", Descripcion = "Control de ansiedad bajo receta" },
                new Categoria { Nombre = "Antihipertensivos", Descripcion = "Tratamiento para presión alta" },
                new Categoria { Nombre = "Antialérgicos", Descripcion = "Para alivio de alergias y rinitis" },
                new Categoria { Nombre = "Antipiréticos", Descripcion = "Reductores de fiebre rápida" },
                new Categoria { Nombre = "Antiácidos", Descripcion = "Alivio de acidez estomacal y reflujo" },
                new Categoria { Nombre = "Mucolíticos", Descripcion = "Jarabe y tratamiento para tos con flema" },
                new Categoria { Nombre = "Antisépticos", Descripcion = "Limpieza y desinfección de heridas" },
                new Categoria { Nombre = "Corticosteroides", Descripcion = "Antiinflamatorios potentes" },
                new Categoria { Nombre = "Diuréticos", Descripcion = "Aumento de la diuresis para retención" },
                new Categoria { Nombre = "Antifúngicos", Descripcion = "Tratamiento contra hongos" },
                new Categoria { Nombre = "Antianémicos", Descripcion = "Suplementos de hierro y ácido fólico" }
            };
            context.Categorias.AddRange(categs);
            await context.SaveChangesAsync();

            // 2. Poblar Estantes (3 originales + 7 nuevos = 10)
            var estantes = new Estante[]
            {
                new Estante { Nombre = "A1", Ubicacion = "Pasillo Principal", Descripcion = "Productos de alta rotación" },
                new Estante { Nombre = "B2-Frio", Ubicacion = "Refrigerador 1", Descripcion = "Requiere cadena de frío" },
                new Estante { Nombre = "C3", Ubicacion = "Almacén Derecho", Descripcion = "Stock de reserva" },
                new Estante { Nombre = "D4", Ubicacion = "Almacén Central", Descripcion = "Almacenamiento masivo general" },
                new Estante { Nombre = "E5", Ubicacion = "Vitrina Segura", Descripcion = "Productos caros bajo llave" },
                new Estante { Nombre = "F6-Frio", Ubicacion = "Refrigerador 2", Descripcion = "Vacunas y biológicos" },
                new Estante { Nombre = "G7", Ubicacion = "Pasillo Secundario", Descripcion = "Productos de mediana rotación" },
                new Estante { Nombre = "H8", Ubicacion = "Mostrador", Descripcion = "Venta directa e impulso" },
                new Estante { Nombre = "I9", Ubicacion = "Bodega Trastienda", Descripcion = "Stock general empaquetado" },
                new Estante { Nombre = "J10-Controlados", Ubicacion = "Caja Fuerte", Descripcion = "Psicotrópicos y recetas retenidas" }
            };
            context.Estantes.AddRange(estantes);
            await context.SaveChangesAsync();

            // 3. Poblar Medicamentos (5 originales + 30 nuevos = 35)
            var meds = new List<Medicamento>
            {
                new Medicamento { Nombre = "Paracetamol 500mg", Precio = 0.50m, Stock = 100, FechaVencimiento = DateTime.Today.AddMonths(18), CategoriaId = categs[0].Id, EstanteId = estantes[0].Id, Estado = true },
                new Medicamento { Nombre = "Amoxicilina Suspensión", Precio = 11.20m, Stock = 15, FechaVencimiento = DateTime.Today.AddDays(-15), CategoriaId = categs[1].Id, EstanteId = estantes[1].Id, Estado = true },
                new Medicamento { Nombre = "Neurobion Inyectable", Precio = 25.00m, Stock = 30, FechaVencimiento = DateTime.Today.AddDays(20), CategoriaId = categs[2].Id, EstanteId = estantes[0].Id, Estado = true },
                new Medicamento { Nombre = "Clonazepam 2mg", Precio = 15.50m, Stock = 4, FechaVencimiento = DateTime.Today.AddMonths(24), CategoriaId = categs[3].Id, EstanteId = estantes[9].Id, Estado = true },
                new Medicamento { Nombre = "Vitamina C Efervescente", Precio = 7.80m, Stock = 80, FechaVencimiento = DateTime.Today.AddDays(55), CategoriaId = categs[2].Id, EstanteId = estantes[2].Id, Estado = true },
                
                // 30 Nuevos:
                new Medicamento { Nombre = "Losartán 50mg", Precio = 12.00m, Stock = 200, FechaVencimiento = DateTime.Today.AddMonths(24), CategoriaId = categs[4].Id, EstanteId = estantes[0].Id, Estado = true },
                new Medicamento { Nombre = "Enalapril 20mg", Precio = 8.50m, Stock = 150, FechaVencimiento = DateTime.Today.AddMonths(12), CategoriaId = categs[4].Id, EstanteId = estantes[6].Id, Estado = true },
                new Medicamento { Nombre = "Cetirizina 10mg", Precio = 5.20m, Stock = 50, FechaVencimiento = DateTime.Today.AddMonths(10), CategoriaId = categs[5].Id, EstanteId = estantes[7].Id, Estado = true },
                new Medicamento { Nombre = "Loratadina 10mg", Precio = 4.80m, Stock = 70, FechaVencimiento = DateTime.Today.AddMonths(20), CategoriaId = categs[5].Id, EstanteId = estantes[0].Id, Estado = true },
                new Medicamento { Nombre = "Ibuprofeno 400mg", Precio = 2.50m, Stock = 300, FechaVencimiento = DateTime.Today.AddMonths(15), CategoriaId = categs[0].Id, EstanteId = estantes[0].Id, Estado = true },
                new Medicamento { Nombre = "Aspirina 100mg", Precio = 1.20m, Stock = 500, FechaVencimiento = DateTime.Today.AddMonths(22), CategoriaId = categs[6].Id, EstanteId = estantes[0].Id, Estado = true },
                new Medicamento { Nombre = "Omeprazol 20mg", Precio = 9.90m, Stock = 120, FechaVencimiento = DateTime.Today.AddMonths(19), CategoriaId = categs[7].Id, EstanteId = estantes[6].Id, Estado = true },
                new Medicamento { Nombre = "Pantoprazol 40mg", Precio = 14.50m, Stock = 90, FechaVencimiento = DateTime.Today.AddMonths(18), CategoriaId = categs[7].Id, EstanteId = estantes[4].Id, Estado = true },
                new Medicamento { Nombre = "Ambroxol Jarabe", Precio = 6.40m, Stock = 45, FechaVencimiento = DateTime.Today.AddMonths(8), CategoriaId = categs[8].Id, EstanteId = estantes[7].Id, Estado = true },
                new Medicamento { Nombre = "Bromhexina Adultos", Precio = 7.10m, Stock = 35, FechaVencimiento = DateTime.Today.AddMonths(7), CategoriaId = categs[8].Id, EstanteId = estantes[6].Id, Estado = true },
                new Medicamento { Nombre = "Povidona Yodada Absoluta", Precio = 4.00m, Stock = 60, FechaVencimiento = DateTime.Today.AddMonths(36), CategoriaId = categs[9].Id, EstanteId = estantes[8].Id, Estado = true },
                new Medicamento { Nombre = "Clorhexidina al 2%", Precio = 8.50m, Stock = 40, FechaVencimiento = DateTime.Today.AddMonths(24), CategoriaId = categs[9].Id, EstanteId = estantes[8].Id, Estado = true },
                new Medicamento { Nombre = "Dexametasona 4mg", Precio = 3.50m, Stock = 20, FechaVencimiento = DateTime.Today.AddMonths(12), CategoriaId = categs[10].Id, EstanteId = estantes[4].Id, Estado = true },
                new Medicamento { Nombre = "Furosemida 40mg", Precio = 6.00m, Stock = 85, FechaVencimiento = DateTime.Today.AddMonths(16), CategoriaId = categs[11].Id, EstanteId = estantes[6].Id, Estado = true },
                new Medicamento { Nombre = "Ketoconazol Crema", Precio = 10.20m, Stock = 110, FechaVencimiento = DateTime.Today.AddMonths(14), CategoriaId = categs[12].Id, EstanteId = estantes[7].Id, Estado = true },
                new Medicamento { Nombre = "Hierro + Ácido Fólico", Precio = 15.00m, Stock = 75, FechaVencimiento = DateTime.Today.AddMonths(20), CategoriaId = categs[13].Id, EstanteId = estantes[2].Id, Estado = true },
                new Medicamento { Nombre = "Diazepam 10mg", Precio = 22.00m, Stock = 10, FechaVencimiento = DateTime.Today.AddMonths(24), CategoriaId = categs[3].Id, EstanteId = estantes[9].Id, Estado = true },
                new Medicamento { Nombre = "Azitromicina 500mg", Precio = 18.50m, Stock = 40, FechaVencimiento = DateTime.Today.AddMonths(11), CategoriaId = categs[1].Id, EstanteId = estantes[4].Id, Estado = true },
                new Medicamento { Nombre = "Ciprofloxacino 500mg", Precio = 14.00m, Stock = 60, FechaVencimiento = DateTime.Today.AddMonths(9), CategoriaId = categs[1].Id, EstanteId = estantes[3].Id, Estado = true },
                new Medicamento { Nombre = "Insulina Glargina", Precio = 45.00m, Stock = 25, FechaVencimiento = DateTime.Today.AddMonths(6), CategoriaId = categs[0].Id, EstanteId = estantes[1].Id, Estado = true }, // Usando Frío
                new Medicamento { Nombre = "Vacuna Antigripal", Precio = 30.00m, Stock = 50, FechaVencimiento = DateTime.Today.AddMonths(3), CategoriaId = categs[6].Id, EstanteId = estantes[5].Id, Estado = true }, // Frío
                new Medicamento { Nombre = "Suplemento Zinc", Precio = 9.00m, Stock = 130, FechaVencimiento = DateTime.Today.AddMonths(28), CategoriaId = categs[2].Id, EstanteId = estantes[7].Id, Estado = true },
                new Medicamento { Nombre = "Naproxeno 550mg", Precio = 8.00m, Stock = 95, FechaVencimiento = DateTime.Today.AddMonths(17), CategoriaId = categs[0].Id, EstanteId = estantes[0].Id, Estado = true },
                new Medicamento { Nombre = "Bismuto Suspensión", Precio = 6.50m, Stock = 30, FechaVencimiento = DateTime.Today.AddMonths(14), CategoriaId = categs[7].Id, EstanteId = estantes[6].Id, Estado = true },
                new Medicamento { Nombre = "Miconazol Aerosol", Precio = 12.80m, Stock = 18, FechaVencimiento = DateTime.Today.AddMonths(22), CategoriaId = categs[12].Id, EstanteId = estantes[3].Id, Estado = true },
                new Medicamento { Nombre = "Betametasona Gotas", Precio = 16.50m, Stock = 22, FechaVencimiento = DateTime.Today.AddMonths(10), CategoriaId = categs[10].Id, EstanteId = estantes[4].Id, Estado = true },
                new Medicamento { Nombre = "Desloratadina Jarabe", Precio = 11.00m, Stock = 42, FechaVencimiento = DateTime.Today.AddMonths(13), CategoriaId = categs[5].Id, EstanteId = estantes[7].Id, Estado = true },
                new Medicamento { Nombre = "Complejo B Forte", Precio = 13.50m, Stock = 66, FechaVencimiento = DateTime.Today.AddMonths(26), CategoriaId = categs[2].Id, EstanteId = estantes[2].Id, Estado = true },
                new Medicamento { Nombre = "Salbutamol Inhalador", Precio = 8.90m, Stock = 55, FechaVencimiento = DateTime.Today.AddMonths(21), CategoriaId = categs[8].Id, EstanteId = estantes[8].Id, Estado = true },
                new Medicamento { Nombre = "Ranitidina 150mg", Precio = 4.50m, Stock = 80, FechaVencimiento = DateTime.Today.AddMonths(8), CategoriaId = categs[7].Id, EstanteId = estantes[3].Id, Estado = true }
            };
            context.Medicamentos.AddRange(meds);
            await context.SaveChangesAsync();
        }
    }
}