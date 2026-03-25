using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using practica.Models;

namespace practica.Data
{
    public class practicaContext : DbContext
    {
        public practicaContext (DbContextOptions<practicaContext> options)
            : base(options)
        {
        }

        public DbSet<practica.Models.Categoria> Categoria { get; set; } = default!;
    }
}
