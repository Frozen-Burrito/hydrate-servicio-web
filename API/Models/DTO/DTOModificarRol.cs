using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos.DTO
{
    public class DTOModificarRol
    {
        [Range(0, (int) RolDeUsuario.ADMINISTRADOR + 1)]
        public RolDeUsuario NuevoRol { get; set; }
    }
}