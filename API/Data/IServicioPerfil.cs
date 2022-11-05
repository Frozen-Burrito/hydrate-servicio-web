using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Modelos.Enums;

namespace ServicioHydrate.Data
{
    public interface IServicioPerfil 
    {
        Task<Perfil> GetPerfilPorId(Guid idCuentaUsuario, int idPerfil);

        Task<DTOConfiguracion> GetConfiguracionDelPerfil(Guid idCuentaUsuario, int idPerfil);

        Task<DTOConfiguracion> ModificarConfiguracionDelPerfil(Guid idCuentaUsuario, int idPerfil, DTOConfiguracion cambiosDeConfiguracion);

        Task ActualizarTokenFCM(Guid idCuentaUsuario, int idPerfil, DTOTokenFCM tokenActualizado);

        Task<DTOPerfil> RegistrarPerfilExistente(DTOPerfil perfil, DTOUsuario cuentaUsuario);

        Task ActualizarPerfil(DTOPerfilModificado cambiosAlPerfil);

        Task EliminarPerfil(Guid idCuentaUsuario, int idPerfil);
    }
}