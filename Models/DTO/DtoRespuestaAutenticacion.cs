using System;

namespace ServicioHydrate.Modelos.DTO
{
    public class DTORespuestaAutenticacion
    {
        public Guid Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}