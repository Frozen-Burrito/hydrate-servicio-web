using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Data 
{
    public interface IServicioUsuarios
    {
        Task<List<DTOUsuario>> GetUsuarios();
        Task<DTOUsuario> GetUsuarioPorId(Guid id);
        Task<DTORespuestaAutenticacion> AutenticarUsuario(DTOPeticionAutenticacion datosUsuario);
        Task<DTOUsuario> RegistrarNuevoUsuario(DTOPeticionAutenticacion datosUsuario);
        Task<DTOUsuario> ActualizarUsuario(DTOUsuario dtoUsuario);
        Task ModificarRolDeUsuario(Guid idUsuario, RolDeUsuario nuevoRol);
        Task EliminarUsuario(Guid id);
    }
}