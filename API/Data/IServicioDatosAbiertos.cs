using System.Collections.Generic;
using System.Threading.Tasks;
using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Modelos.DTO.Datos;
using ServicioHydrate.Modelos.Enums;

#nullable enable
namespace ServicioHydrate.Data 
{
    public interface IServicioDatosAbiertos 
    {
        /// <summary>
        /// Obtiene todos los registros de hidratación que cumplan con los filtros.
        /// </summary>
        /// <remarks>
        /// Si [paramsPagina] no es nulo, el resultado será paginado. Para obtener todos 
        /// los registros, sin importar la cantidad, [paramsPagina] debe ser nulo. 
        /// </remarks>
        /// <param name="filtros">Parámetros que determinan los datos del resultado.</param>
        /// <returns>Los datos abiertos de hidratación.</returns>
        Task<ICollection<DTORegistroDeHidratacion>> GetDatosDeHidratacion(
            FiltrosPorPerfil filtros,
            DTOParamsPagina? paramsPagina 
        );

        /// <summary>
        /// Incluye los registros de hidratación recibidos en la colección
        /// de datos abiertos de hidratación, haciéndolos públicamente accesibles.
        /// </summary>
        /// <param name="datos">Los registros de hidratación aportados.</param>
        Task AportarDatosDeHidratacion(IEnumerable<DTORegistroDeHidratacion> datos);

        /// <summary>
        /// Obtiene todos los registros de actividad física que cumplan con los filtros.
        /// </summary>
        /// <remarks>
        /// Si [paramsPagina] no es nulo, el resultado será paginado. Para obtener todos 
        /// los registros, sin importar la cantidad, [paramsPagina] debe ser nulo. 
        /// </remarks>
        /// <param name="filtros">Los parámetros que determinan los datos del resultado.</param>
        /// <returns>Datos de actividad física.</returns>
        Task<ICollection<DTOActividad>> GetDatosDeActividad(
            FiltrosPorPerfil filtros,
            DTOParamsPagina? paramsPagina
        );

        /// <summary>
        /// Incluye los registros de actividad física de [datos] en la colección
        /// de datos abiertos de actividad física, haciéndolos públicamente accesibles.
        /// </summary>
        /// <param name="datos">Los registros de actividad aportados.</param>
        Task AportarDatosDeActividad(IEnumerable<DTOActividad> datos);

        /// <summary>
        /// Exporta la colección completa de datos abiertos deseada, donde
        /// cada uno de los registros cumple con todos los filtros especificados.
        /// </summary>
        /// <param name="tipoDeDatos">El tipo de datos abiertos a exportar.</param>
        /// <param name="filtros">Determinan los registros que serán incluidos.</param>
        /// <returns>La colección de datos abiertos especificada.</returns>
        Task<IEnumerable<object>> ExportarDatosAbiertos(
            TipoDeDatosAbiertos tipoDeDatos, 
            FiltrosPorPerfil filtros
        );
    }
}
#nullable disable