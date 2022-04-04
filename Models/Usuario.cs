using System;
using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos
{
    // Representa un Usuario en la base de datos.
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        // [MaxLength(32)]
        public string NombreUsuario { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MaxLength(80)]
        public string Password { get; set; }
    }
}