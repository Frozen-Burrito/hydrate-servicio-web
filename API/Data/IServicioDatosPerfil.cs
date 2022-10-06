using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Modelos.DTO.Datos;

namespace ServicioHydrate.Data 
{
    public interface IServicioDatosPerfil 
    {
        /// <summary>
        /// Accede a la colección de entidades de registros de hidratación y 
        /// obtiene los datos de hidratación históricos del usuario especificado.
        /// </summary>
        /// <param name="idUsuario">El ID del usuario.</param>
        /// <param name="paramsPagina">Parámetros del resultado paginado.</param>
        /// <returns>Los registros de hidratación del usuario, en un resultado paginado.</returns>
        Task<DTOResultadoPaginado<DTORegistroDeHidratacion>> GetDatosDeHidratacion(Guid idUsuario, DTOParamsPagina paramsPagina);

        /// <summary>
        /// Agrega todos los [datos] a la colección de entidades para registros 
        /// de hidratación y persiste estos cambios en la base de datos.
        /// </summary>
        /// <param name="datos">Los nuevos registros de hidratación.</param>
        Task AgregarDatosDeHidratacion(IEnumerable<DTORegistroDeHidratacion> datos);

        Task<IEnumerable<DTOMeta>> GetDatosDeMetas(Guid idUsuario, int idPerfil);

        Task AgregarDatosDeMetas(IEnumerable<DTOMeta> datos);

        Task<IEnumerable<DTOResultadoPaginado<DTORegistroActividad>>> GetDatosDeActividad(Guid idUsuario, int idPerfil);

        Task AgregarDatosDeActividad(IEnumerable<DTORegistroActividad> datos);

        Task<IEnumerable<DTOResultadoPaginado<DTORegistroMedico>>> GetRegistrosMedicos(Guid idUsuario, int idPerfil);

        Task AgregarRegistrosMedicos(IEnumerable<DTORegistroMedico> datos);
    
        Task<IEnumerable<DTOResultadoPaginado<DTOReporteSemanal>>> GetDatosSemanales(Guid idUsuario, int idPerfil);

        Task AgregarDatosSemanales(IEnumerable<DTOReporteSemanal> datos);
    }
}