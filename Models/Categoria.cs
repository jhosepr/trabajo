using System.ComponentModel.DataAnnotations;
namespace practica.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
    }
}