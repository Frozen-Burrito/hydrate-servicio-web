using System;
using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos.DTO
{
    // Un objeto de transferencia de datos (DTO) para un Usuario.
    // NO contiene la contraseña (para no mostrarla a todo mundo).
    public class DTOUsuario
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(32)]
        [RegularExpression("^[a-z0-9_-]{4,20}$", ErrorMessage = "El nombre de usuario no tiene un formato válido.")]
        public string NombreUsuario { get; set; }

        [Required]
        [MaxLength(64)]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string Email { get; set; }

        public RolDeUsuario RolDeUsuario { get; set; }
    }
}