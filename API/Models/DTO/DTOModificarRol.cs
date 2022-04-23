using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos.DTO
{
    public class DTOModificarRol
    {
        [Range(0, (int) RolDeUsuario.ADMINISTRADOR)]
        public RolDeUsuario NuevoRol { get; set; }
    }
}