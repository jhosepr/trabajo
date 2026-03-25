using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Farmacia.Models
{
    public class Medicamento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(200)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        [Display(Name = "Stock")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
        [Display(Name = "Fecha de Vencimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaVencimiento { get; set; }

        [StringLength(1000)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; } = true;

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        [Required(ErrorMessage = "El estante es obligatorio")]
        [Display(Name = "Estante")]
        public int EstanteId { get; set; }
        public Estante? Estante { get; set; }

        [NotMapped]
        public bool EstaVencido => FechaVencimiento < DateTime.Today;

        [NotMapped]
        public bool PorVencer30 => !EstaVencido && FechaVencimiento <= DateTime.Today.AddDays(30);

        [NotMapped]
        public bool PorVencer60 => !EstaVencido && FechaVencimiento <= DateTime.Today.AddDays(60);

        [NotMapped]
        public bool PorVencer90 => !EstaVencido && FechaVencimiento <= DateTime.Today.AddDays(90);

        [NotMapped]
        public bool BajoStock => Stock <= 10;
    }
}