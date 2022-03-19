using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioHydrate.Modelos
{
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