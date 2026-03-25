using System.ComponentModel.DataAnnotations;

namespace practica.Models
{
    public class Medicamento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del medicamento es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Vencimiento")]
        public DateTime FechaVencimiento { get; set; }

        public bool Estado { get; set; } = true; // Activo por defecto

        // --- RELACIONES (Punto 3 del PDF) ---

        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }
        public virtual Categoria? Categoria { get; set; }

        [Display(Name = "Estante")]
        public int EstanteId { get; set; }
        public virtual Estante? Estante { get; set; }
    }
}