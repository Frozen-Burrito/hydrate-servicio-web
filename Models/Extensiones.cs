using System.Linq;

using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Modelos
{
    public static class Extensiones
    {
        public static Usuario ComoModelo(this DTOUsuario usuario, string hashContrasenia)
        {
            return new Usuario 
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                Password = hashContrasenia,
            };
        }

        public static DTORespuestaAutenticacion AsRespuestaToken(this Usuario usuario)
        {
            return new DTORespuestaAutenticacion
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                Token = "Token default"
            };
        }
    }
}