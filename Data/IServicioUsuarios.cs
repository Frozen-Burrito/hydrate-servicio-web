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
        Task<Usuario> GetUsuarioPorId(Guid id);
        Task<DTORespuestaAutenticacion> AutenticarUsuario(DTOPeticionAutenticacion infoUsuario);
        Task<DTOUsuario> RegistrarAsync(DTOUsuario usuario);
        Task<DTOUsuario> ActualizarUsuarioAsync(Guid id, DTOUsuario dtoUsuario);
        Task EliminarUsuarioAsync(Guid id);
    }
}