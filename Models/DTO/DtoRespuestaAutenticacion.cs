using System;

namespace ServicioHydrate.Modelos.DTO
{
    /// Representa una respuesta de autenticación, que contiene el JWT 
    /// otorgado por el servicio para el usuario.
    public class DTORespuestaAutenticacion
    {
        public string Token { get; set; }
    }
}