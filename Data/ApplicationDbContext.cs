using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using practica.Models; // Esto es para que encuentre Categoria y Estante

namespace practica.Data
{
    // Heredamos de IdentityDbContext para que ya incluya las tablas de Usuarios y Roles
    public partial class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tus tablas del proyecto de Farmacia
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Estante> Estantes { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
    }
}