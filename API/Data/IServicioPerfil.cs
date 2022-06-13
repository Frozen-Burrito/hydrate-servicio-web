using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Modelos.Enums;

namespace ServicioHydrate.Data
{
    public interface IServicioPerfil 
    {
        Task<DTOPerfil> GetPerfilPorId(Guid idCuentaUsuario, int idPerfil);

        Task<DTOPerfil> RegistrarPerfilExistente(DTOPerfil perfil, DTOUsuario cuentaUsuario);

        Task ActualizarPerfil(DTOPerfil perfil);

        Task EliminarPerfil(Guid idCuentaUsuario, int idPerfil);
    }
}