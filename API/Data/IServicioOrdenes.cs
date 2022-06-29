using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Modelos.Enums;

#nullable enable
namespace ServicioHydrate.Data 
{
    public interface IServicioOrdenes
    {
        /// <summary>
        /// Obtiene una lista paginada con todas las órdenes que cumplan con los parámetros y
        /// los filtros especificados.
        /// </summary>
        /// <remarks>
        /// Es posible filtrar órdenes según el ID, email o nombre de un usuario, así como 
        /// el ID, la fecha y el estado de la orden. 
        /// 
        /// Si fechaDesde es nulo, el rango de fechas abarca todas las órdenes antes de fechaHasta.
        /// Si fechaHasta es nulo, el rango de fechas abarca todas las órdenes después de fechaDesde.
        /// Si ambas fechas son nulas, no se aplica filtro por fecha.
        /// 
        /// Si el estado no es nulo, solo retorna las órdenes que estén en el estado recibido.
        /// </remarks>
        /// <param name="paramsPagina">Los parámetros de paginado para las órdenes.</param>
        /// <param name="paramsOrden">Los filtros aplicados a la búsqueda de órdenes.</param>
        /// <returns>Una lista paginada de órdenes filtradas, en forma de DTOs.</returns>
        Task<ICollection<DTOOrden>> GetOrdenes(DTOParamsPagina? paramsPagina, DTOParamsOrden paramsOrden);

        /// <summary>
        /// Genera un CSV con los datos de todos los órdenes creadas.
        /// </summary>
        /// <remarks>
        /// Lanza un ArgumentException si no existe una orden con el idOrden
        /// recibido.
        /// </remarks>
        /// <param name="idOrden">El Id de la orden.</param>
        /// <returns>La orden con el ID solicitado.</returns>
        Task<IEnumerable<DTOOrden>> ExportarTodasLasOrdenes();

        /// <summary>
        /// Busca una orden con un Id determinado.
        /// </summary>
        /// <remarks>
        /// Lanza un ArgumentException si no existe una orden con el idOrden
        /// recibido.
        /// </remarks>
        /// <param name="idOrden">El Id de la orden.</param>
        /// <returns>La orden con el ID solicitado.</returns>
        Task<DTOOrden> GetOrdenPorId(Guid idOrden);

        /// <summary>
        /// Obtiene estadísticas generales de órdenes de compra.
        /// </summary>
        /// <remarks>
        /// Las estadísticas incluyen:
        /// 
        /// - El número de órdenes completadas.
        /// - El número de órdenes en progreso.
        /// - Las ventas totales, en MXN.
        /// </remarks>
        /// <returns>Un resumen estadístico de todas las órdenes.</returns>
        Task<DTOStatsOrdenes> GetStatsOrdenes();

        /// <summary>
        /// Registra una nueva orden.
        /// </summary>
        /// <param name="nuevaOrden">La información de cantidades y productos de la orden.</param>
        /// <param name="idCliente">El Id del usuario que realiza la orden.</param>
        /// <returns>Los datos de la orden creada.</returns>
        Task<DTOOrden> NuevaOrden(DTONuevaOrden nuevaOrden, Guid idCliente);

        /// <summary>
        /// Cambia el estado de una orden existente.
        /// </summary>
        /// <remarks>
        /// El estado de una orden solo puede ser actualizado en cierto orden:
        /// PENDIENTE -> EN_PROGRESO -> CONCLUIDA O ERROR.
        /// </remarks>
        /// <param name="idOrden">El Id de la orden.</param>
        /// <param name="nuevoEstado">El nuevo estado para la orden.</param>
        /// <returns>Los datos actualizados de la orden.</returns>
        Task<DTOOrden> ModificarEstadoDeOrden(Guid idOrden, EstadoOrden nuevoEstado);
    }
}
#nullable disable
