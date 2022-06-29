using System.Collections.Generic;
using System.Threading.Tasks;

using ServicioHydrate.Modelos.DTO.Datos;
using ServicioHydrate.Modelos.Enums;

namespace ServicioHydrate.Data 
{
    public interface IServicioDatosAbiertos 
    {
        /// <summary>
        /// Obtiene todos los registros de hidratación que cumplan con los filtros, para 
        /// que puedan ser visualizados en una gráfica.
        /// </summary>
        /// <param name="filtros">Parámetros que determinan los datos del resultado.</param>
        /// <returns>Datos de hidratación, en un formato para gráficas.</returns>
        Task<DTOResultadoGrafico<int>> GetDatosDeHidratacion(DTOFiltrosPorPerfil filtros);

        /// <summary>
        /// Incluye los registros de hidratación recibidos en la colección
        /// de datos abiertos de hidratación, haciéndolos públicamente accesibles.
        /// </summary>
        /// <param name="datos">Los registros de hidratación aportados.</param>
        Task AportarDatosDeHidratacion(IEnumerable<DTORegistroDeHidratacion> datos);

        /// <summary>
        /// Obtiene todos los registros de actividad física que cumplan con los filtros, para 
        /// que puedan ser visualizados en una gráfica.
        /// </summary>
        /// <param name="filtros">Los parámetros que determinan los datos del resultado.</param>
        /// <returns>Datos de actividad físicas, en un formato para gráficas.</returns>
        Task<DTOResultadoGrafico<int>> GetDatosDeActividad(DTOFiltrosPorPerfil filtros);

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
            DTOFiltrosPorPerfil filtros
        );
    }
}