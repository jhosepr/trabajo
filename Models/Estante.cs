using System.ComponentModel.DataAnnotations;
namespace practica.Models
{
    public class Estante
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nombre de estante (Ej: A1)")]
        public string Nombre { get; set; }
        public string? Ubicacion { get; set; }
    }
}