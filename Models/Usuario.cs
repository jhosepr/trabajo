using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace practica.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;
    }
    
}
