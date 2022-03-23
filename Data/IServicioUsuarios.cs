using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Data 
{
    public interface IServicioUsuarios
    {
        Task<List<DTOUsuario>> GetUsuariosAsync();
        Task<DTOUsuario> GetUsuarioPorId(Guid id);
        Task<DTORespuestaAutenticacion> AutenticarUsuario(DTOPeticionAutenticacion datosUsuario);
        Task<DTOUsuario> RegistrarAsync(DTOPeticionAutenticacion datosUsuario);
        Task<DTOUsuario> ActualizarUsuarioAsync(DTOUsuario dtoUsuario);
        Task EliminarUsuarioAsync(Guid id);
    }
}