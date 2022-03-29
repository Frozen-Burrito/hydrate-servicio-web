using System;

namespace ServicioHydrate.Modelos.DTO
{
    /// Representa una respuesta de autenticación, que contiene el JWT 
    /// otorgado por el servicio para el usuario.
    public class DTORespuestaAutenticacion
    {
        public Guid Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}