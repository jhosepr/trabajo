using Farmacia.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using practica.Models;

namespace Farmacia.Data
{
    public class practicaContext : IdentityDbContext<Usuario>
    {
        public practicaContext(DbContextOptions<practicaContext> options)
            : base(options) { }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Estante> Estantes { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Medicamento>()
                .HasOne(m => m.Categoria)
                .WithMany(c => c.Medicamentos)
                .HasForeignKey(m => m.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Medicamento>()
                .HasOne(m => m.Estante)
                .WithMany(e => e.Medicamentos)
                .HasForeignKey(m => m.EstanteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}