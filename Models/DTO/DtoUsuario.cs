using System;
using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos.DTO
{
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

        [Required]
        [MaxLength(40)]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,40}$", ErrorMessage = "La contraseña no tiene un formato válido.")]
        public string Password { get; set; }
    }
}