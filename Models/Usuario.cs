using System;
using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos
{
    public class Usuario
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string NombreUsuario { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MaxLength(40)]
        public string Password { get; set; }
    }
}